<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:box="http://ferda.is-a-geek.net" >

<xsl:output method="html" encoding="windows-1250" />

<xsl:template match="box:Boxes">
	<html>
	
		<head>
			<title>Krabičky</title>
		    <link rel="stylesheet" type="text/css" href="styl.css"/>	
		</head>
		
		<body>
			<h1>Obsah:</h1>
			<xsl:for-each select="box:Box">
				<xsl:sort select="box:Identifier"/>
				<p>
					<a href="#{generate-id()}">
						<xsl:value-of select="box:Identifier"></xsl:value-of>
					</a>
				</p>
			</xsl:for-each>
		<xsl:apply-templates><xsl:sort select="box:Identifier"/></xsl:apply-templates>
		</body>
	</html>

</xsl:template>


<xsl:template match="box:Box">
	
	<a name="{generate-id()}"/>
	<xsl:apply-templates/>
</xsl:template>


<xsl:template match="box:Identifier">
	<h1>
		<xsl:apply-templates/>
	</h1>
</xsl:template>


<xsl:template match="box:IconPath">
	<h3>Icon path: </h3>
	<xsl:apply-templates/>
</xsl:template>


<xsl:template match="box:DesignPath">
	<h3>Design path: </h3>
	<xsl:apply-templates/>
</xsl:template>


<xsl:template match="box:Categories">
	<xsl:variable name="pocetKategorii" select="count(child::box:string)"/>
	<h3>Categories (<xsl:value-of select="$pocetKategorii" ></xsl:value-of>):</h3>
			
    <table border="1">
		<tr>
			<th>Name</th>
		</tr>	
	
		<xsl:for-each select="box:string">
			<tr>
				<td class="prvni">
					<xsl:apply-templates/>
				</td>
			</tr>
		</xsl:for-each>
	
	</table>
</xsl:template>


<xsl:template match="box:Actions">
	<xsl:variable name="pocetAkci" select="count(child::box:Action)"/>
	<h3>Actions (<xsl:value-of select="$pocetAkci" ></xsl:value-of>):</h3>
		
    
    <table border="1">
		<tr>
			<th>Name</th>
			<th>Icon Path</th>
		</tr>	
	
		<xsl:for-each select="box:Action">
			<tr>
				<td class="prvni">
					<xsl:value-of select="box:Name"></xsl:value-of>
				</td>
				<td>
					<xsl:value-of select="box:IconPath"></xsl:value-of>
				</td>
			</tr>
		</xsl:for-each>
	
	</table>
</xsl:template>


<xsl:template match="box:Sockets">
	
	<xsl:variable name="pocetSocketu" select="count(child::box:Socket)"/>
	<h3>Sockets (<xsl:value-of select="$pocetSocketu" ></xsl:value-of>):</h3>
			
    
    <table border="1">
		<tr>
			<th>Name</th>
			<xsl:if test="box:Socket/box:DesignPath">
				<th>Design Path</th>
			</xsl:if>	
			<th>Socket Types</th>
			<xsl:if test="box:Socket/box:SettingProperties">
				<th>Setting Properties</th>
			</xsl:if>
			<th>MoreThanOne</th>
		</tr>	
	
		<xsl:for-each select="box:Socket">
			<tr>
				<td class="prvni">
					<b>
						<xsl:value-of select="box:Name"></xsl:value-of>
					</b>
				</td>
				<xsl:if test="parent::box:Sockets/box:Socket/box:DesignPath">
					<td>
						<xsl:value-of select="box:DesignPath"></xsl:value-of>
					</td>
				</xsl:if>
				<td>
					<xsl:apply-templates select="box:SocketTypes"/>
				</td>
				<xsl:if test="parent::box:Sockets/box:Socket/box:SettingProperties">
					<td>
						<xsl:value-of select="box:SettingProperties"></xsl:value-of>
					</td>
				</xsl:if>
				<td>
					<xsl:value-of select="box:MoreThanOne"></xsl:value-of>
				</td>
			</tr>
		</xsl:for-each>
	
	</table>
	
</xsl:template>
    
    
 <!--<xsl:variable name="jmenoProprty" select="box:Name" />

<xsl:value-of select="$jmenoProprty"></xsl:value-of>

PROPRTY : <xsl:value-of select="//box:Property[box:Name='{normalize-space($jmenoProprty)}']"></xsl:value-of>  -->

<!--<xsl:for-each select="//box:Property[name='{box:Name}']"> -->
<!--<xsl:value-of select="box:Name"></xsl:value-of> -->
<!--<xsl:value-of select="//box:Property/box:Name"></xsl:value-of>-->
 
<!--</xsl:for-each> -->


<xsl:template match="box:Properties">
	<xsl:variable name="pocetProperties" select="count(child::box:Property)"/>  
	<h3>Properties (<xsl:value-of select="$pocetProperties"></xsl:value-of>):</h3>	
	
	<table border="1">
		<tr>
			<th>Name</th>
			<th>TypeClassIceId</th>
			<xsl:if test="box:Property/box:CategoryName">
				<th>CategoryName</th>
			</xsl:if>
			<xsl:if test="box:Property/box:SelectOptions">
				<th>SelectOptions</th>
			</xsl:if>
			<th>Visible</th>
		    <th>ReadOnly</th>
			<xsl:if test="box:Property/box:Default">
				<th>Default</th>
			</xsl:if>
			<xsl:if test="box:Property/box:NumericalRestrictions">
				<th>NumericalRestrictions</th>
			</xsl:if>	
			<xsl:if test="box:Property/box:Regexp">
				<th>Regexp</th>
			</xsl:if>	
			<xsl:if test="box:Property/box:SettingModuleIdentifier">
				<th>SettingModuleIdentifier</th>
			</xsl:if>		
		</tr>		
     
		<xsl:for-each select="box:Property">
			<tr>
				<td class="prvni">
					<b>
						<xsl:value-of select="box:Name"></xsl:value-of>
					</b>
				</td>
				<td>
					<xsl:value-of select="box:TypeClassIceId"></xsl:value-of>
				</td>
				<xsl:if test="parent::box:Properties/box:Property/box:CategoryName">
					<td>
						<xsl:value-of select="box:CategoryName"></xsl:value-of>
					</td>
				</xsl:if>
				<xsl:if test="parent::box:Properties/box:Property/box:SelectOptions">
					<td>
						<xsl:apply-templates select="box:SelectOptions"></xsl:apply-templates>
					</td>
				</xsl:if>
				<td>
					<xsl:value-of select="box:Visible"></xsl:value-of>
				</td>
				<td>
					<xsl:value-of select="box:ReadOnly"></xsl:value-of>
				</td>
				<xsl:if test="parent::box:Properties/box:Property/box:Default">
					<td>
						<xsl:value-of select="box:Default"></xsl:value-of>
					</td>
				</xsl:if>
				<xsl:if test="parent::box:Properties/box:Property/box:NumericalRestrictions">
					<td>
						<xsl:apply-templates select="box:NumericalRestrictions"></xsl:apply-templates>
					</td>
				</xsl:if>
				<xsl:if test="parent::box:Properties/box:Property/box:Regexp">
					<td>
						<xsl:value-of select="box:Regexp"></xsl:value-of>
					</td>
				</xsl:if>
				<xsl:if test="parent::box:Properties/box:Property/box:SettingModuleIdentifier">
					<td>
						<xsl:value-of select="box:SettingModuleIdentifier"></xsl:value-of>
					</td>
				</xsl:if>
			</tr>
		</xsl:for-each>
    
    </table>
</xsl:template>
 

<xsl:template match="box:SocketTypes">
	<xsl:for-each select="box:BoxType">
		<xsl:value-of select="box:FunctionIceId"></xsl:value-of>
		<xsl:if test="following-sibling::box:BoxType">
			<xsl:text>, </xsl:text>
		</xsl:if>
	</xsl:for-each>
</xsl:template>


<xsl:template match="box:SelectOptions">
	<xsl:for-each select="box:SelectOption">
		<xsl:value-of select="box:Name"></xsl:value-of>
		<xsl:choose>	
			<xsl:when test="box:DisableProperties">
				(DisableProperties:		
				<xsl:for-each select="box:DisableProperties/box:string">
					<xsl:apply-templates />
					<xsl:choose>
						<xsl:when test="following-sibling::box:string">
							<xsl:text>, </xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>), </xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>				
			</xsl:when>
			<xsl:otherwise>
				<xsl:if test="following-sibling::box:SelectOption">
					<xsl:text>, </xsl:text>
				</xsl:if>	
			</xsl:otherwise>
		</xsl:choose>	
	</xsl:for-each>
</xsl:template>


<xsl:template match="box:NumericalRestrictions">
	<xsl:for-each select="box:Restriction">
		<xsl:if test="box:Integral">
			Integral: <xsl:value-of select="box:Integral"></xsl:value-of>,
		</xsl:if>
		<xsl:if test="box:Floating">
			Floating: <xsl:value-of select="box:Floating"></xsl:value-of>,
		</xsl:if>
		Min: <xsl:value-of select="box:Min"></xsl:value-of>,
		Including: <xsl:value-of select="box:Including"></xsl:value-of>
	</xsl:for-each>
</xsl:template>


<xsl:template match="box:ModulesAskingForCreationSeq">
	<h3>ModulesAskingForCreationSeq: </h3>
	<xsl:for-each select="box:ModulesAskingForCreation">
		<xsl:value-of select="box:Name"></xsl:value-of>
		<xsl:if test="following-sibling::box:ModulesAskingForCreation">
			<xsl:text>, </xsl:text>
		</xsl:if>
    </xsl:for-each>
</xsl:template>



</xsl:stylesheet>
