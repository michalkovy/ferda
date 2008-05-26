<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:fo="http://www.w3.org/1999/XSL/Format"
    xmlns:box="http://ferda.is-a-geek.net">


<xsl:attribute-set name="tabulka.format">
	<xsl:attribute name="table-layout">auto</xsl:attribute>
	<xsl:attribute name="font-family">Helvetica</xsl:attribute>
	<xsl:attribute name="font-size">8pt</xsl:attribute>
	<xsl:attribute name="border-style">solid</xsl:attribute>
    <xsl:attribute name="border-width">2pt</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="tabulka.header.format">
	<xsl:attribute name="background-color">rgb(200,200,200)</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="sloupec.name.format">
	<xsl:attribute name="background-color">rgb(235,235,235)</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="bunka.header.format">
	<xsl:attribute name="padding">3pt</xsl:attribute>
	<xsl:attribute name="border-style">solid</xsl:attribute>
    <xsl:attribute name="border-width">1pt</xsl:attribute>
    <xsl:attribute name="font-weight">bold</xsl:attribute>
    <xsl:attribute name="text-align">center</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="bunka.format">
	<xsl:attribute name="padding">3pt</xsl:attribute>
	<xsl:attribute name="border-style">solid</xsl:attribute>
    <xsl:attribute name="border-width">0.5pt</xsl:attribute>
    <xsl:attribute name="border-color">rgb(200,200,200)</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="nadpis1.format">
	<xsl:attribute name="font-size">20pt</xsl:attribute>
	<xsl:attribute name="font-weight">bold</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="nadpis2.format">
	<xsl:attribute name="font-size">12pt</xsl:attribute>
	<xsl:attribute name="padding-top">5pt</xsl:attribute>
	<xsl:attribute name="font-weight">bold</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="hodnoty.format">
	<xsl:attribute name="font-size">10pt</xsl:attribute>
	<xsl:attribute name="font-weight">normal</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="zahlavi_zapati.format">
	<xsl:attribute name="font-size">8pt</xsl:attribute>
	<xsl:attribute name="font-weight">normal</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="prvni_strana.format">
	<xsl:attribute name="font-size">32pt</xsl:attribute>
	<xsl:attribute name="font-weight">normal</xsl:attribute>
	<xsl:attribute name="text-align">center</xsl:attribute>
</xsl:attribute-set>


<xsl:template match="box:Boxes">
	<fo:root>
		<fo:layout-master-set>

			<fo:simple-page-master master-name="krabicky"
				page-height="210mm" page-width="297mm" 
				margin-left="15mm" margin-right="15mm"
				margin-bottom="5mm" margin-top="5mm">
  
				<fo:region-body margin-bottom="10mm" margin-top="10mm" />        
				<fo:region-before extent="10mm"/>
				<fo:region-after extent="5mm"/>
			</fo:simple-page-master>

			<fo:simple-page-master master-name="obsah"
				page-height="210mm" page-width="297mm" 
				margin-left="20mm" margin-right="20mm"
				margin-bottom="5mm" margin-top="5mm">
				<fo:region-body margin-bottom="10mm" margin-top="10mm" />
			</fo:simple-page-master>
			
			<fo:simple-page-master master-name="prvni_strana"
				page-height="210mm" page-width="297mm" 
				margin-left="20mm" margin-right="20mm"
				margin-bottom="20mm" margin-top="20mm">
				<fo:region-body margin-bottom="10mm" margin-top="10mm" />
			</fo:simple-page-master>

		</fo:layout-master-set>
    
		<fo:page-sequence master-reference="prvni_strana">
			<fo:flow flow-name="xsl-region-body">
				<fo:block xsl:use-attribute-sets="prvni_strana.format">
					Konfigurační soubor krabiček systému Ferda	
				</fo:block>
			</fo:flow>
		</fo:page-sequence>
		
		<fo:page-sequence master-reference="obsah" force-page-count="no-force">

			<fo:flow flow-name="xsl-region-body"
				font-size="10pt"  >

				<fo:block font-size="20pt" margin-bottom="10pt">
					Obsah
				</fo:block>

				<xsl:for-each select="box:Box">
					<xsl:sort select="box:Identifier"/>
						<fo:block text-align-last="justify">
							<fo:basic-link internal-destination="{generate-id()}">
								<xsl:value-of select="box:Identifier"/>
								<fo:leader leader-pattern="dots"/>
								<fo:page-number-citation ref-id="{generate-id(.)}"/>
							</fo:basic-link>
						</fo:block>
					</xsl:for-each> 
			</fo:flow>
		</fo:page-sequence>
 
		<xsl:apply-templates><xsl:sort select="box:Identifier"/></xsl:apply-templates>

	</fo:root>
</xsl:template>

<xsl:template match="box:Box">
	<fo:page-sequence master-reference="krabicky">
		
		<fo:static-content flow-name="xsl-region-after">
			<fo:block text-align="center" xsl:use-attribute-sets="zahlavi_zapati.format">
				<xsl:text>Strana </xsl:text>
				<fo:page-number/>
				<xsl:text> z </xsl:text>
				<fo:page-number-citation ref-id="last_page"/>
			</fo:block>
		</fo:static-content>
			
		<fo:static-content flow-name="xsl-region-before">
			<fo:block text-align="right" xsl:use-attribute-sets="zahlavi_zapati.format">
				<xsl:value-of select="box:Identifier"></xsl:value-of>
            </fo:block>
		</fo:static-content>
		
		<fo:flow flow-name="xsl-region-body">	
			<fo:block id="{generate-id()}"></fo:block>
			<xsl:apply-templates/>
		</fo:flow>
		
	</fo:page-sequence>		
</xsl:template>

<xsl:template match="box:Identifier">
	<fo:block xsl:use-attribute-sets="nadpis1.format" hyphenate="true">
		<xsl:apply-templates/>
	</fo:block>
</xsl:template>

<xsl:template match="box:IconPath">
	<fo:block xsl:use-attribute-sets="nadpis2.format">	
		Icon Path: 
		<fo:inline xsl:use-attribute-sets="hodnoty.format"><xsl:apply-templates/></fo:inline>
	</fo:block>		
</xsl:template>

<xsl:template match="box:DesignPath">
	<fo:block xsl:use-attribute-sets="nadpis2.format">	
		Design Path: 
		<fo:inline xsl:use-attribute-sets="hodnoty.format"><xsl:apply-templates/></fo:inline>
	</fo:block>		
</xsl:template>

<xsl:template match="box:Categories">
	<xsl:variable name="pocetKategorii" select="count(child::box:string)"/>
	<fo:block xsl:use-attribute-sets="nadpis2.format" keep-with-next="always">
		Categories<fo:inline xsl:use-attribute-sets="hodnoty.format">
							<xsl:text> (</xsl:text><xsl:value-of select="$pocetKategorii" ></xsl:value-of><xsl:text>):</xsl:text>
						</fo:inline>			
	</fo:block>	
			
		<fo:table xsl:use-attribute-sets="tabulka.format">
			<fo:table-column xsl:use-attribute-sets="sloupec.name.format" />
			<fo:table-header xsl:use-attribute-sets="tabulka.header.format">
				<fo:table-row>
					<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
						<fo:block>Name</fo:block>
					</fo:table-cell>
				</fo:table-row>	
			</fo:table-header>
			<fo:table-body>
				<xsl:for-each select="box:string">
					<fo:table-row>
						<fo:table-cell xsl:use-attribute-sets="bunka.format">
							<fo:block><xsl:apply-templates/></fo:block>
						</fo:table-cell>
					</fo:table-row>
				</xsl:for-each>
			</fo:table-body>
		</fo:table>
</xsl:template>

<xsl:template match="box:Actions">
	<xsl:variable name="pocetAkci" select="count(child::box:Action)"/>
	<fo:block xsl:use-attribute-sets="nadpis2.format" keep-with-next="always">
			Actions<fo:inline xsl:use-attribute-sets="hodnoty.format">
							<xsl:text> (</xsl:text><xsl:value-of select="$pocetAkci" ></xsl:value-of><xsl:text>):</xsl:text>
						</fo:inline>			
	</fo:block>	
			
		<fo:table xsl:use-attribute-sets="tabulka.format">
			<fo:table-column xsl:use-attribute-sets="sloupec.name.format" />	
			<fo:table-header xsl:use-attribute-sets="tabulka.header.format">
				<fo:table-row>
					<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
						<fo:block>Name</fo:block>
					</fo:table-cell>
					<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
						<fo:block>Icon Path</fo:block>
					</fo:table-cell>
				</fo:table-row>	
			</fo:table-header>
			<fo:table-body>
				<xsl:for-each select="box:Action">
					<fo:table-row>
						<fo:table-cell xsl:use-attribute-sets="bunka.format">
							<fo:block><xsl:value-of select="box:Name"></xsl:value-of></fo:block>
						</fo:table-cell>
						<fo:table-cell xsl:use-attribute-sets="bunka.format">
							<fo:block><xsl:value-of select="box:IconPath"></xsl:value-of></fo:block>
						</fo:table-cell>		
					</fo:table-row>
				</xsl:for-each>
			</fo:table-body>
		</fo:table>
</xsl:template>


<xsl:template match="box:Sockets">
	
	<xsl:variable name="pocetSocketu" select="count(child::box:Socket)"/>
	<fo:block xsl:use-attribute-sets="nadpis2.format" keep-with-next="always">
		Sockets<fo:inline xsl:use-attribute-sets="hodnoty.format">
							<xsl:text> (</xsl:text><xsl:value-of select="$pocetSocketu" ></xsl:value-of><xsl:text>):</xsl:text>
						</fo:inline>			
	</fo:block>
			
     <fo:table  xsl:use-attribute-sets="tabulka.format">
		<fo:table-column xsl:use-attribute-sets="sloupec.name.format" />

		<fo:table-header xsl:use-attribute-sets="tabulka.header.format">
		<fo:table-row>
			<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
				<fo:block>
					Name
				</fo:block>
			</fo:table-cell>
			<xsl:if test="box:Socket/box:DesignPath">
				<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
					<fo:block>	
						Design Path
					</fo:block>
				</fo:table-cell>
			</xsl:if>	
			<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
				<fo:block>	
					Socket Types
				</fo:block>
			</fo:table-cell>
			<xsl:if test="box:Socket/box:SettingProperties">
				<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
					<fo:block>
						Setting Properties
					</fo:block>		
				</fo:table-cell>
			</xsl:if>
			<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
				<fo:block>
					MorethanOne
				</fo:block>
			</fo:table-cell>
		</fo:table-row>	
	</fo:table-header>
	
	<fo:table-body>
		<xsl:for-each select="box:Socket">
			<fo:table-row>
				<fo:table-cell xsl:use-attribute-sets="bunka.format">
					<fo:block>
						<xsl:value-of select="box:Name"></xsl:value-of>
					</fo:block>
				</fo:table-cell>
				<xsl:if test="parent::box:Sockets/box:Socket/box:DesignPath">
					<fo:table-cell xsl:use-attribute-sets="bunka.format">
						<fo:block>
							<xsl:value-of select="box:DesignPath"></xsl:value-of>
						</fo:block>	
					</fo:table-cell>
				</xsl:if>
				<fo:table-cell xsl:use-attribute-sets="bunka.format">
					<fo:block>
						<xsl:apply-templates select="box:SocketTypes"/>
					</fo:block>
				</fo:table-cell>
				<xsl:if test="parent::box:Sockets/box:Socket/box:SettingProperties">
					<fo:table-cell xsl:use-attribute-sets="bunka.format">
						<fo:block>
							<xsl:value-of select="box:SettingProperties"></xsl:value-of>
						</fo:block>
					</fo:table-cell>
				</xsl:if>
				<fo:table-cell xsl:use-attribute-sets="bunka.format">
					<fo:block>
						<xsl:value-of select="box:MoreThanOne"></xsl:value-of>
					</fo:block>	
				</fo:table-cell>
			</fo:table-row>
		</xsl:for-each>
	</fo:table-body>
	</fo:table>
	
</xsl:template>

<xsl:template match="box:Properties">
	<xsl:variable name="pocetProperties" select="count(child::box:Property)"/>  
	<fo:block xsl:use-attribute-sets="nadpis2.format" keep-with-next="always">
			Properties<fo:inline xsl:use-attribute-sets="hodnoty.format">
							<xsl:text> (</xsl:text><xsl:value-of select="$pocetProperties" ></xsl:value-of><xsl:text>):</xsl:text>
						</fo:inline>			
	</fo:block>	
	
	<fo:table xsl:use-attribute-sets="tabulka.format">
		<fo:table-column xsl:use-attribute-sets="sloupec.name.format" />
		<fo:table-header xsl:use-attribute-sets="tabulka.header.format">
		<fo:table-row>
			<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
				<fo:block>
					Name
				</fo:block>
			</fo:table-cell>
			<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
				<fo:block>TypeClassIceId</fo:block></fo:table-cell>
			<xsl:if test="box:Property/box:CategoryName">
				<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
					<fo:block>
						CategoryName
					</fo:block>
				</fo:table-cell>
			</xsl:if>
			<xsl:if test="box:Property/box:SelectOptions">
				<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
					<fo:block>
						SelectOptions
					</fo:block>
				</fo:table-cell>
			</xsl:if>
			<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
				<fo:block>
					Visible
				</fo:block>
			</fo:table-cell>
		    <fo:table-cell xsl:use-attribute-sets="bunka.header.format">
				<fo:block>
					ReadOnly
				</fo:block>
			</fo:table-cell>
			<xsl:if test="box:Property/box:Default">
				<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
					<fo:block>
						Default
					</fo:block>
				</fo:table-cell>
			</xsl:if>
			<xsl:if test="box:Property/box:NumericalRestrictions">
				<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
					<fo:block>
						NumericalRestrictions
					</fo:block>
				</fo:table-cell>
			</xsl:if>	
			<xsl:if test="box:Property/box:Regexp">
				<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
					<fo:block>
						Regexp
					</fo:block>
				</fo:table-cell>
			</xsl:if>	
			<xsl:if test="box:Property/box:SettingModuleIdentifier">
				<fo:table-cell xsl:use-attribute-sets="bunka.header.format">
					<fo:block>
						SettingModuleIdentifier
					</fo:block>
				</fo:table-cell>
			</xsl:if>		
		</fo:table-row>		
        </fo:table-header>
        
        <fo:table-body>
		<xsl:for-each select="box:Property">
			<fo:table-row>
				<fo:table-cell xsl:use-attribute-sets="bunka.format">
					<fo:block>
						<xsl:value-of select="box:Name"></xsl:value-of>
					</fo:block>
				</fo:table-cell>
				<fo:table-cell xsl:use-attribute-sets="bunka.format">
					<fo:block>
						<xsl:value-of select="box:TypeClassIceId"></xsl:value-of>
					</fo:block>
				</fo:table-cell>
				<xsl:if test="parent::box:Properties/box:Property/box:CategoryName">
					<fo:table-cell xsl:use-attribute-sets="bunka.format">
						<fo:block>
							<xsl:value-of select="box:CategoryName"></xsl:value-of>
						</fo:block>
					</fo:table-cell>
				</xsl:if>
				<xsl:if test="parent::box:Properties/box:Property/box:SelectOptions">
					<fo:table-cell xsl:use-attribute-sets="bunka.format">
						<fo:block>
							<xsl:apply-templates select="box:SelectOptions"></xsl:apply-templates>
						</fo:block>
					</fo:table-cell>
				</xsl:if>
				<fo:table-cell xsl:use-attribute-sets="bunka.format">
					<fo:block>
					<xsl:value-of select="box:Visible"></xsl:value-of>
				</fo:block></fo:table-cell>
				<fo:table-cell xsl:use-attribute-sets="bunka.format">
					<fo:block>
						<xsl:value-of select="box:ReadOnly"></xsl:value-of>
					</fo:block>
				</fo:table-cell>
				<xsl:if test="parent::box:Properties/box:Property/box:Default">
					<fo:table-cell xsl:use-attribute-sets="bunka.format">
						<fo:block hyphenate="true">
							<xsl:value-of select="box:Default"></xsl:value-of>
						</fo:block>
					</fo:table-cell>
				</xsl:if>
				<xsl:if test="parent::box:Properties/box:Property/box:NumericalRestrictions">
					<fo:table-cell xsl:use-attribute-sets="bunka.format">
						<fo:block>
							<xsl:apply-templates select="box:NumericalRestrictions"></xsl:apply-templates>
						</fo:block>
					</fo:table-cell>
				</xsl:if>
				<xsl:if test="parent::box:Properties/box:Property/box:Regexp">
					<fo:table-cell xsl:use-attribute-sets="bunka.format">
						<fo:block>
							<xsl:value-of select="box:Regexp"></xsl:value-of>
						</fo:block>
					</fo:table-cell>
				</xsl:if>
				<xsl:if test="parent::box:Properties/box:Property/box:SettingModuleIdentifier">
					<fo:table-cell xsl:use-attribute-sets="bunka.format">
						<fo:block>
							<xsl:value-of select="box:SettingModuleIdentifier"></xsl:value-of>
						</fo:block>
					</fo:table-cell>
				</xsl:if>
			</fo:table-row>
		</xsl:for-each>
    
    	</fo:table-body>
	</fo:table>
</xsl:template>

<xsl:template match="box:SocketTypes">
	<xsl:for-each select="box:BoxType">
		<fo:inline>
			<xsl:value-of select="box:FunctionIceId"></xsl:value-of>
		</fo:inline>
		<xsl:if test="following-sibling::box:BoxType">
			<fo:inline>	
				<xsl:text>, </xsl:text>
			</fo:inline>
		</xsl:if>
	</xsl:for-each>
</xsl:template>


<xsl:template match="box:SelectOptions">
	<xsl:for-each select="box:SelectOption">
		<fo:inline hyphenate="true">
			<xsl:value-of select="box:Name"></xsl:value-of>
		</fo:inline>
		<xsl:choose>	
			<xsl:when test="box:DisableProperties">
				<fo:inline>(DisableProperties:</fo:inline>		
				<xsl:for-each select="box:DisableProperties/box:string">
					<xsl:apply-templates />
					<xsl:choose>
						<xsl:when test="following-sibling::box:string">
							<fo:inline><xsl:text>, </xsl:text></fo:inline>
						</xsl:when>
						<xsl:otherwise>
							<fo:inline><xsl:text>), </xsl:text></fo:inline>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>				
			</xsl:when>
			<xsl:otherwise>
				<xsl:if test="following-sibling::box:SelectOption">
					<fo:inline><xsl:text>, </xsl:text></fo:inline>
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
	<fo:block xsl:use-attribute-sets="nadpis2.format">
		ModulesAskingForCreationSeq: 
		<xsl:for-each select="box:ModulesAskingForCreation">
			<fo:inline xsl:use-attribute-sets="hodnoty.format">
				<xsl:value-of select="box:Name"></xsl:value-of>
				<xsl:if test="following-sibling::box:ModulesAskingForCreation">
					<xsl:text>, </xsl:text>
				</xsl:if>
			</fo:inline>
		</xsl:for-each>
	</fo:block>
	<fo:block id="last_page"/>
</xsl:template>



</xsl:stylesheet>