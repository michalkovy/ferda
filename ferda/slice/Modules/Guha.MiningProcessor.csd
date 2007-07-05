<ClassProject>
  <Language>CSharp</Language>
  <Entities>
    <Entity type="CSharpClass">
      <Name>BitStringIce</Name>
      <Access>Public</Access>
      <Field type="CSharpField">public Ferda.Modules.LongSeq value</Field>
      <Field type="CSharpField">public int length</Field>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="StructType">
      <Name>BitStringIceWithCategoryID</Name>
      <Access>Public</Access>
      <Field type="CSharpField">public BitStringIce bitString</Field>
      <Field type="CSharpField">public string categoryId</Field>
    </Entity>
    <Entity type="StructType">
      <Name>GuidAttributeNamePair</Name>
      <Access>Public</Access>
      <Field type="CSharpField">public Ferda.Modules.GuidStruct id</Field>
      <Field type="CSharpField">public string attributeName</Field>
    </Entity>
    <Entity type="CSharpInterface">
      <Name>SourceDataTableIdProvider</Name>
      <Access>Public</Access>
      <Operation type="CSharpMethod">string GetSourceDataTableIdProvider()</Operation>
    </Entity>
    <Entity type="CSharpInterface">
      <Name>AttributeNameProvider</Name>
      <Access>Public</Access>
      <Operation type="CSharpMethod">GuidAttributeNamePair GetAttributeNames()</Operation>
    </Entity>
    <Entity type="CSharpInterface">
      <Name>BitStringGenerator</Name>
      <Access>Public</Access>
      <Operation type="CSharpMethod">Ferda.Modules.GuidStruct GetAttributeId()</Operation>
      <Operation type="CSharpMethod">Ferda.Guha.Data.CardinalityEnum GetAttributeCardinality()</Operation>
      <Operation type="CSharpMethod">Ferda.Modules.StringSeq GetCategoriesIds()</Operation>
      <Operation type="CSharpMethod">Ferda.Modules.DoubleSeq GetCategoriesNumericValues()</Operation>
      <Operation type="CSharpMethod">BitStringIce GetBitString()</Operation>
      <Operation type="CSharpMethod">Ferda.Modules.StringOpt GetMissingInformationCategory()</Operation>
      <Operation type="CSharpMethod">Ferda.Modules.IntSeq GetCountVector()</Operation>
      <Operation type="CSharpMethod">bool GetNextBitString()</Operation>
      <Operation type="CSharpMethod">Long GetMaxBitStringCount()</Operation>
    </Entity>
    <Entity type="CSharpEnum">
      <Name>ImportanceEnum</Name>
      <Access>Public</Access>
      <Value>Forced</Value>
      <Value>Basic</Value>
      <Value>Auxiliary</Value>
    </Entity>
    <Entity type="CSharpClass">
      <Name>IEntitySetting</Name>
      <Access>Public</Access>
      <Field type="CSharpField">public Ferda.Modules.GuidStruct id</Field>
      <Field type="CSharpField">public ImportanceEnum importance</Field>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="CSharpClass">
      <Name>CoefficientFixedSetSetting</Name>
      <Access>Public</Access>
      <Field type="CSharpField">public Ferda.Modules.StringSeq categoriesIds</Field>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="CSharpEnum">
      <Name>CoefficientTypeEnum</Name>
      <Access>Public</Access>
      <Value>Subsets</Value>
      <Value>SubsetsOneOne</Value>
      <Value>CyclicIntervals</Value>
      <Value>Intervals</Value>
      <Value>Cuts</Value>
      <Value>LeftCuts</Value>
      <Value>RightCuts</Value>
    </Entity>
    <Entity type="CSharpClass">
      <Name>CoefficicentSetting</Name>
      <Access>Public</Access>
      <Field type="CSharpField">public CoefficientTypeEnum coefficientType</Field>
      <Field type="CSharpField">public int maxLength</Field>
      <Field type="CSharpField">public int minLength</Field>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="CSharpClass">
      <Name>ILeafEntitySetting</Name>
      <Access>Public</Access>
      <Field type="CSharpField">public BitStringGenerator generator</Field>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="CSharpClass">
      <Name>ISingleOperandEntitySetting</Name>
      <Access>Public</Access>
      <Field type="CSharpField">public IEntitySetting operand</Field>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="CSharpEnum">
      <Name>SignTypeEnum</Name>
      <Access>Public</Access>
      <Value>Positive</Value>
      <Value>Negative</Value>
      <Value>Both</Value>
    </Entity>
    <Entity type="CSharpClass">
      <Name>NegationSetting</Name>
      <Access>Public</Access>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="CSharpClass">
      <Name>BothSignsSetting</Name>
      <Access>Public</Access>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="CSharpClass">
      <Name>IMultipleOperandEntitySetting</Name>
      <Access>Public</Access>
      <Field type="CSharpField">public IEntitySetting [] operands</Field>
      <Field type="CSharpField">public Ferda.Modules.GuidStructSeqSeq classesOfEquivalence</Field>
      <Field type="CSharpField">public int minLength</Field>
      <Field type="CSharpField">public int maxLength</Field>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="CSharpClass">
      <Name>ConjunctionSetting</Name>
      <Access>Public</Access>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="CSharpClass">
      <Name>DisjunctionSetting</Name>
      <Access>Public</Access>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="CSharpInterface">
      <Name>BitStringGeneratorProvider</Name>
      <Access>Public</Access>
      <Operation type="CSharpMethod">BitStringGenerator GetBitSTringGenerator()</Operation>
    </Entity>
    <Entity type="CSharpInterface">
      <Name>BooleanAttributeSettingFunctions</Name>
      <Access>Public</Access>
      <Operation type="CSharpMethod">IEntitySetting GetEntitySetting()</Operation>
    </Entity>
    <Entity type="CSharpInterface">
      <Name>BooleanAttributeSettingWithGenerationAbilityFunctions</Name>
      <Access>Public</Access>
    </Entity>
    <Entity type="CSharpInterface">
      <Name>EquivalenceClassFunctions</Name>
      <Access>Public</Access>
      <Operation type="CSharpMethod">Ferda.Modules.GuidStructSeq GetEquivalenceClass()</Operation>
    </Entity>
    <Entity type="CSharpInterface">
      <Name>MiningTaskFunctions</Name>
      <Access>Public</Access>
      <Operation type="CSharpMethod">Ferda.Guha.Math.Quantifiers.QuantifierBaseFunctionsPrxSeq GetQuantifiers()</Operation>
      <Operation type="CSharpMethod">string GetResult()</Operation>
    </Entity>
    <Entity type="CSharpEnum">
      <Name>TaskTypeEnum</Name>
      <Access>Public</Access>
      <Value>FourFold</Value>
      <Value>KL</Value>
      <Value>CF</Value>
      <Value>SDFourFold</Value>
      <Value>SDKL</Value>
      <Value>SDCF</Value>
    </Entity>
    <Entity type="CSharpEnum">
      <Name>ResultTypeEnum</Name>
      <Access>Public</Access>
      <Value>Trace</Value>
      <Value>TraceBoolean</Value>
      <Value>TraceReal</Value>
    </Entity>
    <Entity type="CSharpEnum">
      <Name>MarkEnum</Name>
      <Access>Public</Access>
      <Value>Antecedent</Value>
      <Value>Succedent</Value>
      <Value>Condition</Value>
      <Value>RowAttribute</Value>
      <Value>ColumnAttribute</Value>
      <Value>Attribute</Value>
      <Value>FirstSet</Value>
      <Value>SecondSet</Value>
    </Entity>
    <Entity type="StructType">
      <Name>BooleanAttribute</Name>
      <Access>Public</Access>
      <Field type="CSharpField">public MarkEnum mark</Field>
      <Field type="CSharpField">public IEntitySetting settting</Field>
    </Entity>
    <Entity type="StructType">
      <Name>CategorialAttribute</Name>
      <Access>Public</Access>
      <Field type="CSharpField">public MarkEnum mark</Field>
      <Field type="CSharpField">public BitStringGenerator setting</Field>
    </Entity>
    <Entity type="CSharpEnum">
      <Name>TaskEvaluationTypeEnum</Name>
      <Access>Public</Access>
      <Value>FirstN</Value>
      <Value>TopN</Value>
    </Entity>
    <Entity type="CSharpEnum">
      <Name>WorkingWithSecondSetModeEnum</Name>
      <Access>Public</Access>
      <Value>None</Value>
      <Value>Cedent2</Value>
      <Value>Cedent1AndCedent2</Value>
    </Entity>
    <Entity type="StructType">
      <Name>TaskRunParams</Name>
      <Access>Public</Access>
      <Field type="CSharpField">public TaskTypeEnum taskType</Field>
      <Field type="CSharpField">public ResultTypeEnum resultType</Field>
      <Field type="CSharpField">public TaskEvaluationTypeEnum evaluationType</Field>
      <Field type="CSharpField">public Long maxSizeOfResults</Field>
      <Field type="CSharpField">public int skipFirstN</Field>
      <Field type="CSharpField">public WorkingWithSecondSetModeEnum sdWorkingWithSecondSetMode</Field>
    </Entity>
  </Entities>
  <Relations>
    <Relation type="Association" first="1" second="0">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="4" second="2">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="5" second="0">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="7" second="6">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Dependency" first="5" second="4" />
    <Relation type="Dependency" first="4" second="3" />
    <Relation type="Dependency" first="11" second="10" />
    <Relation type="Dependency" first="11" second="8" />
    <Relation type="Association" first="11" second="5">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Dependency" first="7" second="11" />
    <Relation type="Association" first="10" second="9">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Dependency" first="7" second="12" />
    <Relation type="Association" first="12" second="7">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Dependency" first="12" second="14" />
    <Relation type="Dependency" first="12" second="15" />
    <Relation type="Dependency" first="7" second="16" />
    <Relation type="Dependency" first="16" second="17" />
    <Relation type="Dependency" first="16" second="18" />
    <Relation type="Association" first="19" second="5">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Dependency" first="19" second="20" />
    <Relation type="Dependency" first="4" second="20" />
    <Relation type="Association" first="20" second="7">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Dependency" first="20" second="21" />
    <Relation type="Dependency" first="5" second="21" />
    <Relation type="Dependency" first="19" second="23" />
    <Relation type="Dependency" first="4" second="23" />
    <Relation type="Association" first="27" second="7">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="28" second="5">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="31" second="24">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="31" second="25">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="31" second="29">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="27" second="26">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="28" second="26">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="31" second="30">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
  </Relations>
  <Positions>
    <Shape>
      <Location left="28" top="18" />
      <Size width="193" height="124" />
    </Shape>
    <Shape>
      <Location left="323" top="18" />
      <Size width="162" height="124" />
    </Shape>
    <Shape>
      <Location left="28" top="317" />
      <Size width="193" height="124" />
    </Shape>
    <Shape>
      <Location left="343" top="598" />
      <Size width="240" height="93" />
    </Shape>
    <Shape>
      <Location left="323" top="458" />
      <Size width="283" height="93" />
    </Shape>
    <Shape>
      <Location left="323" top="168" />
      <Size width="342" height="223" />
    </Shape>
    <Shape>
      <Location left="703" top="18" />
      <Size width="162" height="128" />
    </Shape>
    <Shape>
      <Location left="923" top="28" />
      <Size width="187" height="124" />
    </Shape>
    <Shape>
      <Location left="1096" top="687" />
      <Size width="264" height="144" />
    </Shape>
    <Shape>
      <Location left="724" top="536" />
      <Size width="137" height="193" />
    </Shape>
    <Shape>
      <Location left="903" top="496" />
      <Size width="230" height="141" />
    </Shape>
    <Shape>
      <Location left="923" top="230" />
      <Size width="187" height="107" />
    </Shape>
    <Shape>
      <Location left="1247" top="40" />
      <Size width="193" height="107" />
    </Shape>
    <Shape>
      <Location left="1247" top="168" />
      <Size width="162" height="128" />
    </Shape>
    <Shape>
      <Location left="1514" top="40" />
      <Size width="115" height="80" />
    </Shape>
    <Shape>
      <Location left="1514" top="139" />
      <Size width="115" height="80" />
    </Shape>
    <Shape>
      <Location left="1247" top="317" />
      <Size width="192" height="167" />
    </Shape>
    <Shape>
      <Location left="1514" top="317" />
      <Size width="130" height="80" />
    </Shape>
    <Shape>
      <Location left="1514" top="417" />
      <Size width="130" height="80" />
    </Shape>
    <Shape>
      <Location left="343" top="715" />
      <Size width="268" height="90" />
    </Shape>
    <Shape>
      <Location left="751" top="864" />
      <Size width="212" height="115" />
    </Shape>
    <Shape>
      <Location left="687" top="1030" />
      <Size width="334" height="70" />
    </Shape>
    <Shape>
      <Location left="18" top="564" />
      <Size width="304" height="100" />
    </Shape>
    <Shape>
      <Location left="228" top="864" />
      <Size width="433" height="115" />
    </Shape>
    <Shape>
      <Location left="18" top="687" />
      <Size width="162" height="176" />
    </Shape>
    <Shape>
      <Location left="18" top="878" />
      <Size width="162" height="128" />
    </Shape>
    <Shape>
      <Location left="1130" top="1044" />
      <Size width="162" height="216" />
    </Shape>
    <Shape>
      <Location left="840" top="1115" />
      <Size width="162" height="124" />
    </Shape>
    <Shape>
      <Location left="840" top="1259" />
      <Size width="162" height="124" />
    </Shape>
    <Shape>
      <Location left="18" top="1030" />
      <Size width="162" height="108" />
    </Shape>
    <Shape>
      <Location left="18" top="1180" />
      <Size width="217" height="125" />
    </Shape>
    <Shape>
      <Location left="301" top="1044" />
      <Size width="364" height="216" />
    </Shape>
    <Connection>
      <StartNode isHorizontal="True" location="63" />
      <EndNode isHorizontal="True" location="18" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="66" />
      <EndNode isHorizontal="True" location="25" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="132" />
      <EndNode isHorizontal="True" location="18" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="85" />
      <EndNode isHorizontal="True" location="22" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="False" location="101" />
      <EndNode isHorizontal="False" location="101" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="False" location="98" />
      <EndNode isHorizontal="False" location="98" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="False" location="58" />
      <EndNode isHorizontal="False" location="78" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="56" />
      <EndNode isHorizontal="False" location="47" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="69" />
      <EndNode isHorizontal="True" location="29" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="False" location="47" />
      <EndNode isHorizontal="False" location="47" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="66" />
      <EndNode isHorizontal="True" location="21" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="33" />
      <EndNode isHorizontal="True" location="21" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="71" />
      <EndNode isHorizontal="True" location="38" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="27" />
      <EndNode isHorizontal="True" location="27" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="42" />
      <EndNode isHorizontal="True" location="30" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="33" />
      <EndNode isHorizontal="True" location="26" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="28" />
      <EndNode isHorizontal="True" location="28" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="29" />
      <EndNode isHorizontal="True" location="27" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="66" />
      <EndNode isHorizontal="True" location="29" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="22" />
      <EndNode isHorizontal="True" location="28" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="32" />
      <EndNode isHorizontal="False" location="115" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="67" />
      <EndNode isHorizontal="False" location="116" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="False" location="50" />
      <EndNode isHorizontal="False" location="114" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="141" />
      <EndNode isHorizontal="False" location="31" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="False" location="81" />
      <EndNode isHorizontal="False" location="81" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="False" location="46" />
      <EndNode isHorizontal="False" location="27" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="90" />
      <EndNode isHorizontal="False" location="131" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="82" />
      <EndNode isHorizontal="False" location="310" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="65" />
      <EndNode isHorizontal="True" location="25" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="82" />
      <EndNode isHorizontal="True" location="24" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="100" />
      <EndNode isHorizontal="True" location="24" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="66" />
      <EndNode isHorizontal="True" location="16" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="63" />
      <EndNode isHorizontal="True" location="16" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="154" />
      <EndNode isHorizontal="True" location="18" />
    </Connection>
  </Positions>
</ClassProject>