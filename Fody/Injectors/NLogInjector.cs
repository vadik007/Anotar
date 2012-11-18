using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class NLogInjector : IInjector
{
    public void Init(AssemblyDefinition reference, ModuleDefinition moduleDefinition)
    {
        var logManagerType = reference.MainModule.Types.First(x => x.Name == "LogManager");
        var getLoggerDefinition = logManagerType.Methods.First(x => x.Name == "GetLogger" && x.IsMatch("String"));
        buildLoggerMethod = moduleDefinition.Import(getLoggerDefinition);
        var getLoggerGenericDefinition = logManagerType.Methods.First(x => x.Name == "GetCurrentClassLogger");
        buildLoggerGenericMethod = moduleDefinition.Import(getLoggerGenericDefinition);
        var loggerTypeDefinition = reference.MainModule.Types.First(x => x.Name == "Logger");

        DebugMethod = moduleDefinition.Import(loggerTypeDefinition.FindMethod("Debug", "String"));
        DebugExceptionMethod = moduleDefinition.Import(loggerTypeDefinition.FindMethod("DebugException", "String", "Exception"));
        InfoMethod = moduleDefinition.Import(loggerTypeDefinition.FindMethod("Info", "String"));
        InfoExceptionMethod = moduleDefinition.Import(loggerTypeDefinition.FindMethod("InfoException", "String", "Exception"));
        WarnMethod = moduleDefinition.Import(loggerTypeDefinition.FindMethod("Warn", "String"));
        WarnExceptionMethod = moduleDefinition.Import(loggerTypeDefinition.FindMethod("WarnException", "String", "Exception"));
        ErrorMethod = moduleDefinition.Import(loggerTypeDefinition.FindMethod("Error", "String"));
        ErrorExceptionMethod = moduleDefinition.Import(loggerTypeDefinition.FindMethod("ErrorException", "String", "Exception"));
        LoggerType = moduleDefinition.Import(loggerTypeDefinition);
    }

    public MethodReference DebugMethod { get; set; }
    public MethodReference DebugExceptionMethod { get; set; }
    public MethodReference InfoMethod { get; set; }
    public MethodReference InfoExceptionMethod { get; set; }
    public MethodReference WarnMethod { get; set; }
    public MethodReference WarnExceptionMethod { get; set; }
    public MethodReference ErrorMethod { get; set; }
    public MethodReference ErrorExceptionMethod { get; set; }

    public TypeReference LoggerType { get; set; }

    MethodReference buildLoggerMethod;
    MethodReference buildLoggerGenericMethod;
    

    public IAssemblyResolver AssemblyResolver;

    public void AddField(TypeDefinition type, MethodDefinition constructor, FieldDefinition fieldDefinition)
    {
        var instructions = constructor.Body.Instructions;

        if (type.HasGenericParameters)
        {
            instructions.Insert(0, Instruction.Create(OpCodes.Call, buildLoggerGenericMethod));
            instructions.Insert(1, Instruction.Create(OpCodes.Stsfld, fieldDefinition));
        }
        else
        {
            instructions.Insert(0, Instruction.Create(OpCodes.Ldstr, type.FullName));
            instructions.Insert(1, Instruction.Create(OpCodes.Call, buildLoggerMethod));
            instructions.Insert(2, Instruction.Create(OpCodes.Stsfld, fieldDefinition));
        }

    }

    public string ReferenceName { get { return "NLog"; } }
}