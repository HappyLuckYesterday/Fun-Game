using Rhisis.Core.Extensions;
using System.IO;

namespace Rhisis.Game.Resources;

public sealed class GameResourcePaths
{
    public static readonly string DataPath = Path.Combine(EnvironmentExtension.GetCurrentEnvironementDirectory(), "data");
    public static readonly string DialogsPath = Path.Combine(DataPath, "dialogs");
    public static readonly string ResourcePath = Path.Combine(DataPath, "res");
    public static readonly string MapsPath = Path.Combine(DataPath, "maps");
    public static readonly string ShopsPath = Path.Combine(DataPath, "shops");
    public static readonly string QuestsPath = Path.Combine(DataPath, "quests");
    public static readonly string DataSub0Path = Path.Combine(ResourcePath, "data");
    public static readonly string DataSub1Path = Path.Combine(ResourcePath, "dataSub1");
    public static readonly string DataSub2Path = Path.Combine(ResourcePath, "dataSub2");
    public static readonly string MoversPropPath = Path.Combine(DataSub0Path, "propMover.txt");
    public static readonly string MoversPropExPath = Path.Combine(DataSub0Path, "propMoverEx.inc");
    public static readonly string ItemsPropPath = Path.Combine(DataSub2Path, "propItem.txt");
    public static readonly string WorldScriptPath = Path.Combine(DataSub0Path, "World.inc");
    public static readonly string JobPropPath = Path.Combine(DataSub1Path, "propJob.inc");
    public static readonly string TextClientPath = Path.Combine(DataSub1Path, "textClient.inc");
    public static readonly string ExpTablePath = Path.Combine(DataSub0Path, "expTable.inc");
    public static readonly string SkillPropPath = Path.Combine(DataSub0Path, "propSkill.txt");
    public static readonly string SkillPropAddPath = Path.Combine(DataSub0Path, "propSkillAdd.csv");

    // Rhisis related
    public static readonly string DeathPenalityPath = Path.Combine(ResourcePath, "deathPenality.json");
    public static readonly string JobsDefinitionsPath = Path.Combine(ResourcePath, "jobsDefinitions.json");
}