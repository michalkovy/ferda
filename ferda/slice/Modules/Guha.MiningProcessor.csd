<ClassProject>
  <Language>CSharp</Language>
  <Entities>
    <Entity type="Class">
      <Name>BitStringIce</Name>
      <Access>Public</Access>
      <Member type="Field">public Ferda.Modules.LongSeq value</Member>
      <Member type="Field">public int length</Member>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="Structure">
      <Name>BitStringIceWithCategoryID</Name>
      <Access>Public</Access>
      <Member type="Field">public BitStringIce bitString</Member>
      <Member type="Field">public string categoryId</Member>
    </Entity>
    <Entity type="Structure">
      <Name>GuidAttributeNamePair</Name>
      <Access>Public</Access>
      <Member type="Field">public Ferda.Modules.GuidStruct id</Member>
      <Member type="Field">public string attributeName</Member>
    </Entity>
    <Entity type="Interface">
      <Name>SourceDataTableIdProvider</Name>
      <Access>Public</Access>
      <Member type="Method">string GetSourceDataTableIdProvider()</Member>
    </Entity>
    <Entity type="Interface">
      <Name>AttributeNameProvider</Name>
      <Access>Public</Access>
      <Member type="Method">GuidAttributeNamePair GetAttributeNames()</Member>
    </Entity>
    <Entity type="Interface">
      <Name>BitStringGenerator</Name>
      <Access>Public</Access>
      <Member type="Method">Ferda.Modules.GuidStruct GetAttributeId()</Member>
      <Member type="Method">Ferda.Guha.Data.CardinalityEnum GetAttributeCardinality()</Member>
      <Member type="Method">Ferda.Modules.StringSeq GetCategoriesIds()</Member>
      <Member type="Method">Ferda.Modules.DoubleSeq GetCategoriesNumericValues()</Member>
      <Member type="Method">BitStringIce GetBitString()</Member>
      <Member type="Method">Ferda.Modules.StringOpt GetMissingInformationCategory()</Member>
      <Member type="Method">Ferda.Modules.IntSeq GetCountVector()</Member>
      <Member type="Method">bool GetNextBitString()</Member>
      <Member type="Method">Long GetMaxBitStringCount()</Member>
    </Entity>
    <Entity type="Enum">
      <Name>ImportanceEnum</Name>
      <Access>Public</Access>
      <Value>Forced</Value>
      <Value>Basic</Value>
      <Value>Auxiliary</Value>
    </Entity>
    <Entity type="Class">
      <Name>IEntitySetting</Name>
      <Access>Public</Access>
      <Member type="Field">public Ferda.Modules.GuidStruct id</Member>
      <Member type="Field">public ImportanceEnum importance</Member>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="Class">
      <Name>CoefficientFixedSetSetting</Name>
      <Access>Public</Access>
      <Member type="Field">public Ferda.Modules.StringSeq categoriesIds</Member>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="Enum">
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
    <Entity type="Class">
      <Name>CoefficicentSetting</Name>
      <Access>Public</Access>
      <Member type="Field">public CoefficientTypeEnum coefficientType</Member>
      <Member type="Field">public int maxLength</Member>
      <Member type="Field">public int minLength</Member>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="Class">
      <Name>ILeafEntitySetting</Name>
      <Access>Public</Access>
      <Member type="Field">public BitStringGenerator generator</Member>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="Class">
      <Name>ISingleOperandEntitySetting</Name>
      <Access>Public</Access>
      <Member type="Field">public IEntitySetting operand</Member>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="Enum">
      <Name>SignTypeEnum</Name>
      <Access>Public</Access>
      <Value>Positive</Value>
      <Value>Negative</Value>
      <Value>Both</Value>
    </Entity>
    <Entity type="Class">
      <Name>NegationSetting</Name>
      <Access>Public</Access>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="Class">
      <Name>BothSignsSetting</Name>
      <Access>Public</Access>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="Class">
      <Name>IMultipleOperandEntitySetting</Name>
      <Access>Public</Access>
      <Member type="Field">public IEntitySetting [] operands</Member>
      <Member type="Field">public int minLength</Member>
      <Member type="Field">public int maxLength</Member>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="Class">
      <Name>ConjunctionSetting</Name>
      <Access>Public</Access>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="Class">
      <Name>DisjunctionSetting</Name>
      <Access>Public</Access>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="Interface">
      <Name>BitStringGeneratorProvider</Name>
      <Access>Public</Access>
      <Member type="Method">BitStringGenerator GetBitSTringGenerator()</Member>
    </Entity>
    <Entity type="Interface">
      <Name>BooleanAttributeSettingFunctions</Name>
      <Access>Public</Access>
      <Member type="Method">IEntitySetting GetEntitySetting()</Member>
    </Entity>
    <Entity type="Interface">
      <Name>BooleanAttributeSettingWithGenerationAbilityFunctions</Name>
      <Access>Public</Access>
    </Entity>
    <Entity type="Interface">
      <Name>MiningTaskFunctions</Name>
      <Access>Public</Access>
      <Member type="Method">Ferda.Guha.Math.Quantifiers.QuantifierBaseFunctionsPrxSeq GetQuantifiers()</Member>
      <Member type="Method">string GetResult()</Member>
    </Entity>
    <Entity type="Enum">
      <Name>TaskTypeEnum</Name>
      <Access>Public</Access>
      <Value>FourFold</Value>
      <Value>KL</Value>
      <Value>CF</Value>
      <Value>SDFourFold</Value>
      <Value>SDKL</Value>
      <Value>SDCF</Value>
      <Value>ETree</Value>
    </Entity>
    <Entity type="Enum">
      <Name>ResultTypeEnum</Name>
      <Access>Public</Access>
      <Value>Trace</Value>
      <Value>TraceBoolean</Value>
      <Value>TraceReal</Value>
    </Entity>
    <Entity type="Enum">
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
      <Value>TargetClassificationAttribute</Value>
      <Value>BranchingAttributes</Value>
    </Entity>
    <Entity type="Structure">
      <Name>BooleanAttribute</Name>
      <Access>Public</Access>
      <Member type="Field">public MarkEnum mark</Member>
      <Member type="Field">public IEntitySetting settting</Member>
    </Entity>
    <Entity type="Structure">
      <Name>CategorialAttribute</Name>
      <Access>Public</Access>
      <Member type="Field">public MarkEnum mark</Member>
      <Member type="Field">public BitStringGenerator setting</Member>
    </Entity>
    <Entity type="Enum">
      <Name>TaskEvaluationTypeEnum</Name>
      <Access>Public</Access>
      <Value>FirstN</Value>
      <Value>TopN</Value>
    </Entity>
    <Entity type="Enum">
      <Name>WorkingWithSecondSetModeEnum</Name>
      <Access>Public</Access>
      <Value>None</Value>
      <Value>Cedent2</Value>
      <Value>Cedent1AndCedent2</Value>
    </Entity>
    <Entity type="Structure">
      <Name>TaskRunParams</Name>
      <Access>Public</Access>
      <Member type="Field">public TaskTypeEnum taskType</Member>
      <Member type="Field">public ResultTypeEnum resultType</Member>
      <Member type="Field">public TaskEvaluationTypeEnum evaluationType</Member>
      <Member type="Field">public Long maxSizeOfResults</Member>
      <Member type="Field">public int skipFirstN</Member>
      <Member type="Field">public WorkingWithSecondSetModeEnum sdWorkingWithSecondSetMode</Member>
    </Entity>
    <Entity type="Interface">
      <Name>MiningProcessorFunctions</Name>
      <Access>Public</Access>
      <Member type="Method">string Run()</Member>
      <Member type="Method">BitStringIceWithCategoryId GetNextBitString()</Member>
      <Member type="Method">string ETreeRun()</Member>
    </Entity>
    <Entity type="Structure">
      <Name>ETreeTaskRunParams</Name>
      <Access>Public</Access>
      <Member type="Field">public CategorialAttributeSeq branchingAttributes</Member>
      <Member type="Field">public CategorialAttribute targetClassificationAttribute</Member>
      <Member type="Field">public Ferda.Guha.Math.Quantifiers.QuantifierBase quantifiers</Member>
      <Member type="Field">public int minimalNodeImpurity</Member>
      <Member type="Field">public int minimalNodeFrequency</Member>
      <Member type="Field">public BranchingStoppingCriterionEnum branchingStoppingCriterion</Member>
      <Member type="Field">public int maximalTreeDepth</Member>
      <Member type="Field">public int noAttributesForBranching</Member>
      <Member type="Field">public long maxNumberOfHypotheses</Member>
      <Member type="Field">public bool onlyFullTree</Member>
    </Entity>
    <Entity type="Enum">
      <Name>BranchingStoppingCriterionEnum</Name>
      <Access>Public</Access>
      <Value>MinimalNodePurity</Value>
      <Value>MinimalNodeFrequency</Value>
      <Value>MinimalNodePurityORMinimalNodeFrequency</Value>
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
    <Relation type="Dependency" first="19" second="22" />
    <Relation type="Association" first="26" second="7">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="27" second="5">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="30" second="23">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="30" second="24">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="30" second="28">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="26" second="25">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="27" second="25">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="30" second="29">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Dependency" first="3" second="4" />
    <Relation type="Dependency" first="4" second="5" />
    <Relation type="Dependency" first="4" second="22" />
    <Relation type="Association" first="27" second="32">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Association" first="33" second="32">
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
      <Location left="323" top="8" />
      <Size width="193" height="124" />
    </Shape>
    <Shape>
      <Location left="18" top="186" />
      <Size width="193" height="124" />
    </Shape>
    <Shape>
      <Location left="323" top="149" />
      <Size width="240" height="93" />
    </Shape>
    <Shape>
      <Location left="362" top="287" />
      <Size width="283" height="93" />
    </Shape>
    <Shape>
      <Location left="395" top="429" />
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
      <Location left="818" top="536" />
      <Size width="137" height="193" />
    </Shape>
    <Shape>
      <Location left="1007" top="491" />
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
      <Size width="115" height="85" />
    </Shape>
    <Shape>
      <Location left="1514" top="139" />
      <Size width="115" height="85" />
    </Shape>
    <Shape>
      <Location left="1247" top="317" />
      <Size width="206" height="167" />
    </Shape>
    <Shape>
      <Location left="1514" top="317" />
      <Size width="130" height="85" />
    </Shape>
    <Shape>
      <Location left="1514" top="417" />
      <Size width="130" height="85" />
    </Shape>
    <Shape>
      <Location left="323" top="698" />
      <Size width="268" height="90" />
    </Shape>
    <Shape>
      <Location left="751" top="864" />
      <Size width="212" height="115" />
    </Shape>
    <Shape>
      <Location left="751" top="1030" />
      <Size width="334" height="70" />
    </Shape>
    <Shape>
      <Location left="220" top="864" />
      <Size width="433" height="115" />
    </Shape>
    <Shape>
      <Location left="18" top="687" />
      <Size width="162" height="184" />
    </Shape>
    <Shape>
      <Location left="18" top="878" />
      <Size width="162" height="128" />
    </Shape>
    <Shape>
      <Location left="1138" top="1115" />
      <Size width="162" height="235" />
    </Shape>
    <Shape>
      <Location left="840" top="1115" />
      <Size width="162" height="124" />
    </Shape>
    <Shape>
      <Location left="840" top="1260" />
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
    <Shape>
      <Location left="1096" top="888" />
      <Size width="176" height="116" />
    </Shape>
    <Shape>
      <Location left="335" top="1308" />
      <Size width="352" height="255" />
    </Shape>
    <Shape>
      <Location left="18" top="1325" />
      <Size width="264" height="126" />
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
      <EndNode isHorizontal="False" location="184" />
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
    <Connection>
      <StartNode isHorizontal="False" location="125" />
      <EndNode isHorizontal="False" location="85" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="False" location="105" />
      <EndNode isHorizontal="False" location="73" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="False" location="19" />
      <EndNode isHorizontal="False" location="74" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="17" />
      <EndNode isHorizontal="True" location="66" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="24" />
      <EndNode isHorizontal="True" location="148" />
    </Connection>
  </Positions>
</ClassProject>