using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MecanimEventEmitTypes
{
	Default,
	Upwards,
	Broadcast,
}

public class MecanimEventEmitter : MonoBehaviour 
{

	public UnityEngine.Object animatorController;
	public Animator animator;
	public MecanimEventEmitTypes emitType = MecanimEventEmitTypes.Default;
	public MecanimEventData data;
	
	private Dictionary<int, Dictionary<int, Dictionary<int, List<MecanimEvent>>>> loadedData;
	
	private Dictionary<int, Dictionary<int, AnimatorStateInfo>> lastStates = new Dictionary<int, Dictionary<int, AnimatorStateInfo>>();
	
	void Start() 
	{
		if (animator == null) {
			Debug.LogWarning(string.Format("GameObject:{0} cannot find animator component.",this.transform.name));
			this.enabled = false;
			return;
		}
				
		if (animatorController == null) {
			Debug.LogWarning("Please assgin animator in editor. Add emitter at runtime is not currently supported.");
			this.enabled = false;
			return;
		}
		
		if (data == null){
			this.enabled = false;
			return;
		}
			
		LoadData();
	}
	
	void LoadData() {
		if (data == null)
			return;
		
		loadedData = new Dictionary<int, Dictionary<int, Dictionary<int, List<MecanimEvent>>>>();
		
		MecanimEventDataEntry[] entries = data.data;
		
		foreach(MecanimEventDataEntry entry in entries) {
			int animatorControllerId = entry.animatorController.GetInstanceID();
			
			if (!loadedData.ContainsKey(animatorControllerId))
				loadedData[animatorControllerId] = new Dictionary<int, Dictionary<int, List<MecanimEvent>>>();
			
			if (!loadedData[animatorControllerId].ContainsKey(entry.layer)) {
				loadedData[animatorControllerId][entry.layer] = new Dictionary<int, List<MecanimEvent>>();
			}
			
			loadedData[animatorControllerId][entry.layer][entry.stateNameHash] = new List<MecanimEvent>(entry.events);
		}
	}
	
	public MecanimEvent[] GetEvents(int animatorControllerId, Animator animator) {
		List<MecanimEvent> allEvents = new List<MecanimEvent>();
		
		int animatorHash = animator.GetHashCode();
		if (!lastStates.ContainsKey(animatorHash))
			lastStates[animatorHash] = new Dictionary<int, AnimatorStateInfo>();
		
		int layerCount = animator.layerCount;
		
		Dictionary<int, AnimatorStateInfo> lastLayerState = lastStates[animatorHash];
		
		for (int layer = 0; layer < layerCount; layer++) {
			if (!lastLayerState.ContainsKey(layer)) {
				lastLayerState[layer] = new AnimatorStateInfo();
			}
			
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
			
			int lastLoop = (int)lastLayerState[layer].normalizedTime;
			int currLoop = (int)stateInfo.normalizedTime;
			float lastNormalizedTime = lastLayerState[layer].normalizedTime - lastLoop;
			float currNormalizedTime = stateInfo.normalizedTime - currLoop;
			
			if (lastLayerState[layer].nameHash == stateInfo.nameHash) {
				if (stateInfo.loop == true) {
					if (lastLoop == currLoop) {
						allEvents.AddRange(CollectEvents(animator, animatorControllerId, layer, stateInfo.nameHash, stateInfo.tagHash, lastNormalizedTime, currNormalizedTime));
					}
					else {
						allEvents.AddRange(CollectEvents(animator, animatorControllerId, layer, stateInfo.nameHash, stateInfo.tagHash, lastNormalizedTime, 1.00001f));
						allEvents.AddRange(CollectEvents(animator, animatorControllerId, layer, stateInfo.nameHash, stateInfo.tagHash, 0.0f, currNormalizedTime));
					}
				}
				else {
					float start = Mathf.Clamp01(lastLayerState[layer].normalizedTime);
					float end = Mathf.Clamp01(stateInfo.normalizedTime);
					
					if (lastLoop == 0 && currLoop == 0) {
						if (start != end)
							allEvents.AddRange(CollectEvents(animator, animatorControllerId, layer, stateInfo.nameHash, stateInfo.tagHash, start, end));
					}
					else if (lastLoop == 0 && currLoop > 0) {
						allEvents.AddRange(CollectEvents(animator, animatorControllerId, layer, lastLayerState[layer].nameHash, lastLayerState[layer].tagHash, start, 1.00001f));
					}
					else {
						
					}
				}
			}
			else {
				
				allEvents.AddRange(CollectEvents(animator, animatorControllerId, layer, stateInfo.nameHash, stateInfo.tagHash, 0.0f, currNormalizedTime));
			
				if (!lastLayerState[layer].loop) {
					allEvents.AddRange(CollectEvents(animator, animatorControllerId, layer, lastLayerState[layer].nameHash, lastLayerState[layer].tagHash, lastNormalizedTime, 1.00001f, true));
				}
			}
			
			lastLayerState[layer] = stateInfo;
		}
		
		return allEvents.ToArray();
	}
	
	private MecanimEvent[] CollectEvents(Animator animator, int animatorControllerId, int layer, int nameHash, int tagHash,
								float normalizedTimeStart, float normalizedTimeEnd, bool onlyCritical = false) 
	{
		List<MecanimEvent> events;		
		
		if (loadedData.ContainsKey(animatorControllerId) &&
			loadedData[animatorControllerId].ContainsKey(layer) && 
			loadedData[animatorControllerId][layer].ContainsKey(nameHash)) {
			
			events = loadedData[animatorControllerId][layer][nameHash];
		}
		else {
			return new MecanimEvent[0];
		}
		
		List<MecanimEvent> ret = new List<MecanimEvent>();
		
		foreach (MecanimEvent e in events) {
			
			if (e.normalizedTime >= normalizedTimeStart && e.normalizedTime < normalizedTimeEnd) {
				if (e.condition.Test(animator)) {
					
					if (onlyCritical && !e.critical)
						continue;
					
					MecanimEvent finalEvent = new MecanimEvent(e);
					EventContext context = new EventContext();
					context.controllerId = animatorControllerId;
					context.layer = layer;
					context.stateHash = nameHash;
					context.tagHash = tagHash;
					
					finalEvent.SetContext(context);
					
					ret.Add(finalEvent);
				}
			}
		}
		
		return ret.ToArray();
	}
	
	void Update () {
		MecanimEvent[] events = GetEvents(animatorController.GetInstanceID(), animator);
		
		foreach (MecanimEvent e in events) {
			
			MecanimEvent.SetCurrentContext(e);
			
			switch(emitType)
			{
			case MecanimEventEmitTypes.Upwards:
				if (e.paramType != MecanimEventParamTypes.None)
					SendMessageUpwards(e.FunctionName, e.parameter, SendMessageOptions.DontRequireReceiver);
				else
					SendMessageUpwards(e.FunctionName, SendMessageOptions.DontRequireReceiver);				
				break;
				
			case MecanimEventEmitTypes.Broadcast:
				if (e.paramType != MecanimEventParamTypes.None)
					BroadcastMessage(e.FunctionName, e.parameter, SendMessageOptions.DontRequireReceiver);
				else
					BroadcastMessage(e.FunctionName, SendMessageOptions.DontRequireReceiver);				
				break;
				
			default:
				if (e.paramType != MecanimEventParamTypes.None)
					SendMessage(e.FunctionName, e.parameter, SendMessageOptions.DontRequireReceiver);
				else
					SendMessage(e.FunctionName, SendMessageOptions.DontRequireReceiver);
				break;
			}
		}
	}
}
