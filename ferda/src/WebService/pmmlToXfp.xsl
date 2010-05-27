<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" exclude-result-prefixes="xs" version="1.0"
    xmlns:pmml="http://www.dmg.org/PMML-3_2">
    <xsl:param name="dataProvider"/>
    <xsl:param name="connectionString"/>
    <xsl:param name="dataTableName"/>
    <xsl:param name="primaryKeyStringSeq"/>
    <xsl:output encoding="UTF-8" indent="yes" method="xml" />
    
    <xsl:template match="/">
        <Project>
            <Views/>
            <Boxes>
                <xsl:call-template name="StaticBoxes"/>
                <xsl:apply-templates select="pmml:PMML/pmml:DataDictionary/pmml:DataField"/>
                <xsl:apply-templates select="pmml:PMML/pmml:TransformationDictionary/pmml:DerivedField"/>
                <xsl:apply-templates select="pmml:PMML/pmml:AssociationModel"/>
            </Boxes>
        </Project>
    </xsl:template>
    
    <xsl:template name="StaticBoxes">
        <xsl:call-template name="Box">
            <xsl:with-param name="creatorIdentifier">DataPreparation.DataSource.Database</xsl:with-param>
            <xsl:with-param name="projectIdentifier">2</xsl:with-param>
            <xsl:with-param name="propertySets">
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">ProviderInvariantName</xsl:with-param>
                    <xsl:with-param name="value" select="$dataProvider"/>
                    <xsl:with-param name="xsiType">StringValueT</xsl:with-param>
                </xsl:call-template>
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">ConnectionString</xsl:with-param>
                    <xsl:with-param name="value" select="$connectionString"/>
                    <xsl:with-param name="xsiType">StringValueT</xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
        </xsl:call-template>
        <xsl:call-template name="Box">
            <xsl:with-param name="creatorIdentifier">DataPreparation.DataSource.DataTable</xsl:with-param>
            <xsl:with-param name="projectIdentifier">3</xsl:with-param>
            <xsl:with-param name="propertySets">
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">Name</xsl:with-param>
                    <xsl:with-param name="value" select="$dataTableName"/>
                    <xsl:with-param name="xsiType">StringValueT</xsl:with-param>
                </xsl:call-template>
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">PrimaryKeyColumns</xsl:with-param>
                    <xsl:with-param name="value" select="$primaryKeyStringSeq"/>
                    <xsl:with-param name="xsiType">StringSeqValueT</xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
            <xsl:with-param name="connections">
                <xsl:call-template name="Connection">
                    <xsl:with-param name="boxProjectIdentifier">2</xsl:with-param>
                    <xsl:with-param name="socketName">Database</xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
        </xsl:call-template>
        <xsl:call-template name="Box">
            <xsl:with-param name="creatorIdentifier">SemanticWeb.PMMLBuilder</xsl:with-param>
            <xsl:with-param name="projectIdentifier">4</xsl:with-param>
            <xsl:with-param name="propertySets">
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">PMMLFile</xsl:with-param>
                    <xsl:with-param name="value">c:\test.pmml</xsl:with-param>
                    <xsl:with-param name="xsiType">StringValueT</xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
            <xsl:with-param name="connections">
                <xsl:call-template name="Connection">
                    <xsl:with-param name="boxProjectIdentifier">1</xsl:with-param>
                    <xsl:with-param name="socketName">4FTTask</xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
        </xsl:call-template>
    </xsl:template>
    
    <xsl:template match="pmml:PMML/pmml:AssociationModel">
        <xsl:call-template name="Box">
            <xsl:with-param name="creatorIdentifier">GuhaMining.Tasks.FourFold</xsl:with-param>
            <xsl:with-param name="projectIdentifier">1</xsl:with-param>
            <xsl:with-param name="connections">
                <xsl:if test="pmml:Extension[@name='TaskSetting']/pmml:Antecedent">
                    <xsl:call-template name="Connection">
                        <xsl:with-param name="socketName">Antecedent</xsl:with-param>
                        <xsl:with-param name="boxProjectIdentifier"><xsl:value-of select="100 + pmml:Extension[@name='TaskSetting']/pmml:Antecedent"/></xsl:with-param>
                    </xsl:call-template>
                </xsl:if>
                <xsl:if test="pmml:Extension[@name='TaskSetting']/pmml:Consequent">
                    <xsl:call-template name="Connection">
                        <xsl:with-param name="socketName">Succedent</xsl:with-param>
                        <xsl:with-param name="boxProjectIdentifier"><xsl:value-of select="100 + pmml:Extension[@name='TaskSetting']/pmml:Consequent"/></xsl:with-param>
                    </xsl:call-template>
                </xsl:if>
                <xsl:if test="pmml:Extension[@name='TaskSetting']/pmml:Condition">
                    <xsl:call-template name="Connection">
                        <xsl:with-param name="socketName">Condition</xsl:with-param>
                        <xsl:with-param name="boxProjectIdentifier"><xsl:value-of select="100 + pmml:Extension[@name='TaskSetting']/pmml:Condition"/></xsl:with-param>
                    </xsl:call-template>
                </xsl:if>
                <xsl:for-each select="pmml:Extension[@name='QuantifierThreshold']">
                    <xsl:call-template name="Connection">
                        <xsl:with-param name="socketName">Quantifiers</xsl:with-param>
                        <xsl:with-param name="boxProjectIdentifier"><xsl:value-of select="10 + count(preceding-sibling::pmml:Extension[@name='QuantifierThreshold'])"/></xsl:with-param>
                    </xsl:call-template>
                </xsl:for-each>
            </xsl:with-param>
            <xsl:with-param name="propertySets">
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">ExecutionType</xsl:with-param>
                    <xsl:with-param name="xsiType">StringValueT</xsl:with-param>
                    <xsl:with-param name="value">FirstN</xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
        </xsl:call-template>
        <xsl:apply-templates select="pmml:Extension[@name='TaskSetting']/pmml:BasicBooleanAttributeSettings/pmml:BasicBooleanAttributeSetting" />
        <xsl:apply-templates select="pmml:Extension[@name='TaskSetting']/pmml:DerivedBooleanAttributeSettings/pmml:DerivedBooleanAttributeSetting" />
        <xsl:apply-templates select="pmml:Extension[@name='QuantifierThreshold']" />
    </xsl:template>
    
    <xsl:template match="pmml:PMML/pmml:DataDictionary/pmml:DataField">
        <xsl:call-template name="Box">
            <xsl:with-param name="creatorIdentifier">DataPreparation.DataSource.Column</xsl:with-param>
            <xsl:with-param name="projectIdentifier"><xsl:value-of select="2000 + count(preceding-sibling::node())"/></xsl:with-param>
            <xsl:with-param name="propertySets">
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">SelectExpression</xsl:with-param>
                    <xsl:with-param name="value"><xsl:value-of select="@name"/></xsl:with-param>
                    <xsl:with-param name="xsiType">StringValueT</xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
            <xsl:with-param name="connections">
                <xsl:call-template name="Connection">
                    <xsl:with-param name="socketName">DataTable</xsl:with-param>
                    <xsl:with-param name="boxProjectIdentifier">3</xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
        </xsl:call-template>        
    </xsl:template>
    
    <xsl:template match="pmml:PMML/pmml:TransformationDictionary/pmml:DerivedField">
        <xsl:variable name="field">
            <xsl:choose>
                <xsl:when test="pmml:Discretize">
                    <xsl:value-of select="pmml:Discretize/@field"/>
                </xsl:when>
                <xsl:when test="pmml:MapValues">
                    <xsl:value-of select="pmml:MapValues/pmml:FieldColumnPair/@field"/>
                </xsl:when>
                <xsl:otherwise><xsl:message terminate="yes">Unsupported DerivedField</xsl:message></xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <xsl:call-template name="Box">
            <xsl:with-param name="creatorIdentifier">
                <xsl:choose>
                    <xsl:when test="pmml:Discretize">
                        DataPreparation.Categorization.EquidistantIntervals
                    </xsl:when>
                    <xsl:when test="pmml:MapValues">
                        DataPreparation.Categorization.EachValueOneCategory
                    </xsl:when>
                    <xsl:otherwise><xsl:message terminate="yes">Unsupported DerivedField</xsl:message></xsl:otherwise>
                </xsl:choose>
            </xsl:with-param>
            <xsl:with-param name="projectIdentifier"><xsl:value-of select="1000 + count(preceding-sibling::node())"/></xsl:with-param>
            <xsl:with-param name="propertySets">
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">NameInBooleanAttributes</xsl:with-param>
                    <xsl:with-param name="value"><xsl:value-of select="@name"/></xsl:with-param>
                    <xsl:with-param name="xsiType">StringValueT</xsl:with-param>
                </xsl:call-template>
                <xsl:if test="pmml:Discretize">
                    <xsl:call-template name="PropertySet">
                        <xsl:with-param name="propertyName">CountOfCategories</xsl:with-param>
                        <xsl:with-param name="value"><xsl:value-of select="count(pmml:Discretize/pmml:DiscretizeBin)"/></xsl:with-param>
                        <xsl:with-param name="xsiType">LongValueT</xsl:with-param>
                    </xsl:call-template>
                </xsl:if>
            </xsl:with-param>
            <xsl:with-param name="connections">
                <xsl:call-template name="Connection">
                    <xsl:with-param name="socketName">Column</xsl:with-param>
                    <xsl:with-param name="boxProjectIdentifier"><xsl:value-of select="20000 + count(/pmml:PMML/pmml:DataDictionary/pmml:DataField[@name=$field]/preceding-sibling::node())"/></xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
        </xsl:call-template>        
    </xsl:template>
    
    <xsl:template match="pmml:Extension[@name='TaskSetting']/pmml:BasicBooleanAttributeSettings/pmml:BasicBooleanAttributeSetting">
        <xsl:variable name="attribute" select="pmml:Attribute"/>
        <xsl:call-template name="Box">
            <xsl:with-param name="creatorIdentifier">GuhaMining.AtomSetting</xsl:with-param>
            <xsl:with-param name="projectIdentifier"><xsl:value-of select="100 + @id"/></xsl:with-param>
            <xsl:with-param name="propertySets">
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">CoefficientType</xsl:with-param>
                    <xsl:with-param name="xsiType">StringValueT</xsl:with-param>
                    <xsl:with-param name="value"><xsl:value-of select="pmml:CoefficientType"/></xsl:with-param>
                </xsl:call-template>
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">MaximalLength</xsl:with-param>
                    <xsl:with-param name="xsiType">IntValueT</xsl:with-param>
                    <xsl:with-param name="value"><xsl:value-of select="pmml:MaximalLength"/></xsl:with-param>
                </xsl:call-template>
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">MinimalLength</xsl:with-param>
                    <xsl:with-param name="xsiType">IntValueT</xsl:with-param>
                    <xsl:with-param name="value"><xsl:value-of select="pmml:MinimalLength"/></xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
            <xsl:with-param name="connections">
                <xsl:call-template name="Connection">
                    <xsl:with-param name="boxProjectIdentifier"><xsl:value-of select="1000 + count(/pmml:PMML/pmml:TransformationDictionary/pmml:DerivedField[@name=$attribute]/preceding-sibling::node())"/></xsl:with-param>
                    <xsl:with-param name="socketName">BitStringGenerator</xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
        </xsl:call-template>
    </xsl:template>
    
    <xsl:template match="pmml:Extension[@name='TaskSetting']/pmml:DerivedBooleanAttributeSettings/pmml:DerivedBooleanAttributeSetting">
        <xsl:call-template name="Box">
            <xsl:with-param name="creatorIdentifier">
                <xsl:choose>
                    <xsl:when test="@type = 'Disjunction'">GuhaMining.DisjunctionSetting</xsl:when>
                    <xsl:when test="@type = 'Conjunction'">GuhaMining.ConjuctionSetting</xsl:when>
                    <xsl:otherwise><xsl:message terminate="yes">Unsupported type: <xsl:value-of select="@type"/></xsl:message></xsl:otherwise>
                </xsl:choose>
            </xsl:with-param>
            <xsl:with-param name="projectIdentifier"><xsl:value-of select="100 + @id"/></xsl:with-param>
            <xsl:with-param name="connections">
                <xsl:for-each select="pmml:BooleanAttributeId">
                    <xsl:call-template name="Connection">
                        <xsl:with-param name="boxProjectIdentifier"><xsl:value-of select="100 + ."/></xsl:with-param>
                        <xsl:with-param name="socketName">BooleanAttributeSetting</xsl:with-param>
                    </xsl:call-template>
                </xsl:for-each>
            </xsl:with-param>
            <xsl:with-param name="propertySets">
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">MaximalLength</xsl:with-param>
                    <xsl:with-param name="xsiType">IntValueT</xsl:with-param>
                    <xsl:with-param name="value"><xsl:value-of select="pmml:MaximalLength"/></xsl:with-param>
                </xsl:call-template>
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">MinimalLength</xsl:with-param>
                    <xsl:with-param name="xsiType">IntValueT</xsl:with-param>
                    <xsl:with-param name="value"><xsl:value-of select="pmml:MinimalLength"/></xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
        </xsl:call-template>
    </xsl:template>
    
    <xsl:template match="pmml:Extension[@name='QuantifierThreshold']">
        <xsl:call-template name="Box">
            <xsl:with-param name="creatorIdentifier">
                <xsl:choose>
                    <xsl:when test="@value = 'FoundedImplication'">GuhaMining.Quantifiers.FourFold.Implicational.FoundedImplication</xsl:when>
                    <xsl:when test="@value = 'Base'">GuhaMining.Quantifiers.FourFold.Others.Base</xsl:when>
                    <xsl:when test="@value = 'AboveAverageDependence'">GuhaMining.Quantifiers.FourFold.Others.AboveAverageDependence</xsl:when>
                    <xsl:otherwise><xsl:message terminate="yes">Unsupported value: <xsl:value-of select="@value"/></xsl:message></xsl:otherwise>
                </xsl:choose>
            </xsl:with-param>
            <xsl:with-param name="projectIdentifier"><xsl:value-of select="10 + count(preceding-sibling::pmml:Extension[@name='QuantifierThreshold'])"/></xsl:with-param>
            <xsl:with-param name="propertySets">
                <xsl:call-template name="PropertySet">
                    <xsl:with-param name="propertyName">Treshold</xsl:with-param>
                    <xsl:with-param name="xsiType">DoubleValueT</xsl:with-param>
                    <xsl:with-param name="value"><xsl:value-of select="pmml:Threshold"/></xsl:with-param>
                </xsl:call-template>
            </xsl:with-param>
        </xsl:call-template>
    </xsl:template>
    
    <xsl:template name="Box">
        <xsl:param name="creatorIdentifier"/>
        <xsl:param name="projectIdentifier"/>
        <xsl:param name="connections"/>
        <xsl:param name="propertySets"/>
        <Box>
            <CreatorIdentifier><xsl:value-of select="$creatorIdentifier"/></CreatorIdentifier>
            <ProjectIdentifier><xsl:value-of select="$projectIdentifier" /></ProjectIdentifier>
            <Connections><xsl:copy-of select="$connections" /></Connections>
            <PropertySets ><xsl:copy-of select="$propertySets" /></PropertySets>
            <SockedProperties />
        </Box>
    </xsl:template>
    
    <xsl:template name="Connection">
        <xsl:param name="socketName"/>
        <xsl:param name="boxProjectIdentifier"/>
        <Connection>
            <SocketName><xsl:value-of select="$socketName"/></SocketName>
            <BoxProjectIdentifier><xsl:value-of select="$boxProjectIdentifier"/></BoxProjectIdentifier>
        </Connection>
    </xsl:template>
    
    <xsl:template name="PropertySet">
        <xsl:param name="propertyName"/>
        <xsl:param name="value"/>
        <xsl:param name="xsiType"/>
        <PropertySet>
            <PropertyName><xsl:value-of select="$propertyName"/></PropertyName>
            <Value>
                <xsl:attribute name="xsi:type"><xsl:value-of select="$xsiType"/></xsl:attribute>
                <Value>
                    <xsl:variable name="type">
                        <xsl:choose>
                            <xsl:when test="$xsiType = 'DoubleValueT'">doubleValue</xsl:when>
                            <xsl:when test="$xsiType = 'IntValueT'">intValue</xsl:when>
                            <xsl:when test="$xsiType = 'LongValueT'">longValue</xsl:when>
                            <xsl:when test="$xsiType = 'StringValueT'">stringValue</xsl:when>
                            <xsl:when test="$xsiType = 'StringSeqValueT'">stringSeqValue</xsl:when>
                            <xsl:otherwise><xsl:message terminate="yes">Unsupported xsiType in input: <xsl:value-of select="$xsiType"></xsl:value-of></xsl:message></xsl:otherwise>
                        </xsl:choose>
                    </xsl:variable>
                    
                    <xsl:element name="{$type}">
                        <xsl:value-of select="$value"/>
                    </xsl:element>
                </Value>
            </Value>
        </PropertySet>
    </xsl:template>
</xsl:stylesheet>
