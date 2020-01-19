using UnityEngine;
using System.Collections;

public enum UNIT_TYPE
{
	None = 0,
	HERO, 	// 英雄
	NPC,// 关卡NPC
	WARRIOR,// 战斗神将
	ATTACK_NPC,// 攻击NPC
	TREASUREBOX,// 宝箱
	GRASS,//草垛
}

public class SERVER_DEFINE
{
	public static string IP = "127.0.0.1";
	public static int Port = 2001;
	public static string Account = "cj001";
	public static string Password = "123";
}

public class COMMDEF
{
    public static bool IS_ASSETBUNDLE_LOAD = false;
    public static bool IS_RES_MD5 = false;
	public static bool IS_DEBUG = true;

	public static int INIT_SCENE_ID = 1;
	public static int CITY_SCENE_ID = 2;

	public const string RESPATH_CONFIG_DATA = "Config";

	public const string RESPATH_EFFECT = "Effect";
	public const string RESPATH_SKILL_EFFECT = "Effect/Skill";
	public const string RESPATH_BUFFER_EFFECT = "Effect/Buffer";

    public const string RESPATH_MODEL = "Model";

    public const string RESPATH_UI = "UI";
	public const string RESPATH_UI_PANEL = "UI/Panel";
	public const string RESPATH_UI_ICON = "UI/icon";
	public const string RESPATH_UI_CARD = "UI/Card";
	public const string RESPATH_UI_SKILL_ICON = "UI/Icon/Skill";
	public const string RESPATH_UI_WARRIOR_ICON = "UI/Icon/Warrior";
	public const string RESPATH_UI_SKILL = "UI/Skill";
	public const string RESPATH_UI_LOADING = "UI/LoadingTex";
	public const string RESPATH_UI_QUALITY_CARD = "UI/Quality/Card";
	public const string RESPATH_UI_CARD_IMAGE = "UI/Card/Image";
	public const string RESPATH_UI_CARD_CAMP = "UI/Card/Camp";
	public const string RESPATH_UI_CARD_STAR_BACK = "UI/Card/StarLevelBack";
	public const string RESPATH_UI_CARD_STAR_SIDE = "UI/Card/StarLevelSide";
	public const string RESPATH_UI_ITEM = "UI/Icon/Item";
	public const string RESPATH_UI_CAPITAL = "UI/Icon/Capital";

	public const string RESPATH_HUD = "HUD";
	//---------------------------------------------------------
	/// <summary>
	/// 角色模型 
	/// </summary>
	public const string RESPATH_WARRIOR = "Act/Warrior";
	public const string RESPATH_NPC = "Act/Npc";
	public const string RESPATH_ACT = "Act";
	//----------------------------------------------------------	
	
    public const string RESPATH_SCENE = "Scene";

    public const string ASSET_DATA = "LoadAsset";
    public const string TEXT_TYPE = "TextAsset";

	public const string XML_HELPER_TEXT = "HelperData";
	public const string XML_NPC_TEXT = "NpcData";
	public const string XML_BUFF_TEXT = "BuffData";
	public const string XML_TREASUREBOX_TEXT = "TreasureBoxData";
	public const string XML_GRASS_TEXT = "GrassData";
	public const string XML_ATTACKNPC_TEXT = "AttackNpcData";
	public const string XML_EFFECT_TEXT = "EffectData";
	public const string XML_SKILL2DEFFECT_TEXT = "Skill2DEffectData";
	public const string XML_CAMERASHAKE_TEXT = "CameraShakeData";
	public const string XML_LOCAL_TEXT = "LocalText";
	public const string XML_SYSTEMINFO_TEXT = "SystemInfoData";
	public const string XML_WARRIOR_TEXT = "WarriorData";
    public const string XML_SKILL_TEXT = "SkillData";
    public const string XML_SCENE_TEXT = "SceneData";
    public const string XML_DUNGEON_TEXT = "DungeonData"; 
	public const string XML_BATTLE_TEXT = "BattleData"; 
	public const string XML_RANDFIRSTNAMEDATA_TEXT = "RandFirstName";
	public const string XML_RANDSECONDNAMEDATA_TEXT = "RandSecondName";
	public const string XML_TACTICS_TEXT = "TacticsData";
	public const string XML_TACTICS_PROP_TEXT = "TacticsPropData";
	public const string XML_SHOP_TEXT = "ShopData";
	public const string XML_LEVEL_TEXT = "LevelData";
    public const string XML_ITEM_TEXT = "Item";
    public const string XML_BUILDING_TEXT = "Building";
	public const string XML_CAPITAL_TEXT = "CapitalData";
	public const string XML_BOSSAI_TEXT = "BossAIData";
	public const string XML_CARDEXTRACT_TEXT = "CardExtractData";
    
	public const int MAX_STAR_LEVEL = 6;
}

public delegate void LoadCallBack(params object[] args);

/// <summary>
/// 对象标志名;
/// </summary>
public class OBJECT_SNAME
{
	public const string NAME = "Name";
	public const string COIN = "CapitalType0";  //铜钱
	public const string SLIVER = "CapitalType1";    //银子
	public const string GOLD = "CapitalType2";  //金子
	public const string DRAFTS = "CapitalType3";    //银票
	public const string LEVEL = "Level";
	public const string JOB = "Job";
	public const string MAXHP = "MaxHP";
	public const string HP = "HP";
	public const string ATK = "Atk";
	public const string DEF = "Def";
	public const string HP_GROW = "HPGrow";
	public const string ATK_GROW = "AtkGrow";
	public const string DEF_GROW = "DefGrow";

	public const string DODGE = "Dodge";
	public const string CRITICAL = "Critical";
	public const string EXP = "Exp";
	public const string UPGRADE_EXP = "UpgradeExp";
	public const string CAMP = "Camp";
	public const string DEAD = "Dead";
	
	public const string CONFIGID = "ConfigID";
	
	public const string AMOUNT = "Amount";
    public const string MAXAMOUNT = "MaxAmount";
}
