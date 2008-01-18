<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:fo="http://www.w3.org/1999/XSL/Format"
                version="1.0">

<xsl:template match="/">
  <fo:root>
    <fo:layout-master-set>
      <fo:simple-page-master page-width="210mm" 
	  						 page-height="297mm"
							 margin-top="1cm"
							 margin-bottom="1cm"
							 margin-left="1cm"
							 margin-right="1cm"
                             master-name="my-first">
		<fo:region-body margin="2.5cm"/>
		<fo:region-before extent="2cm"/>
		<fo:region-after extent="2cm"/>
		<fo:region-start extent="0cm"/>
		<fo:region-end extent="0cm"/>
	  </fo:simple-page-master>
      <fo:simple-page-master page-width="210mm" 
	  						 page-height="297mm"
							 margin-top="1cm"
							 margin-bottom="1cm"
							 margin-left="1cm"
							 margin-right="1cm"
                             master-name="my-master">
		<fo:region-body margin="2.5cm"/>
		<fo:region-before extent="2cm"/>
		<fo:region-after extent="2cm"/>
		<fo:region-start extent="0cm"/>
		<fo:region-end extent="0cm"/>
	  </fo:simple-page-master>
	</fo:layout-master-set>

    <fo:page-sequence master-reference="my-first">
      <fo:flow flow-name="xsl-region-body" 
               font-family="Helvetica">
        <fo:block>
          <xsl:apply-templates select="//articleinfo/title" mode="hlavni"/>
        </fo:block>
        <fo:block>
          <xsl:apply-templates select="//abstract/para" mode="hlavni"/>
        </fo:block>		
      </fo:flow>
    </fo:page-sequence>
	
	
    <fo:page-sequence master-reference="my-master">
 	  <fo:static-content flow-name="xsl-region-before">
        <fo:block font-family="Helvetica" 
				font-size="10pt"
           		text-align="center">
		  <xsl:apply-templates select="//articleinfo/title" mode="zahlavi"/>				
        </fo:block>
        <fo:block text-align="center">
	      <fo:leader leader-pattern="rule" leader-length="18cm" />
        </fo:block>	  		
   	  </fo:static-content>
 	  <fo:static-content flow-name="xsl-region-after">
        <fo:block text-align="center">
	      <fo:leader leader-pattern="rule" leader-length="18cm" />
        </fo:block>	  
        <fo:block font-family="Helvetica" 
				font-size="8pt"
           		text-align="center">
		  <fo:page-number />				
        </fo:block>
   	  </fo:static-content>		  	  
      <fo:flow flow-name="xsl-region-body" 
               font-family="Helvetica"
               font-size="8pt">
        <fo:block>
          <xsl:apply-templates/>
        </fo:block>
      </fo:flow>
    </fo:page-sequence>
  </fo:root>
</xsl:template>

<xsl:template match="//articleinfo/title" mode="zahlavi">
  <fo:block font-family="Helvetica"
            font-size="10pt"
            font-weight="bold"
			font-style="italic"
			text-align="center">
    <xsl:apply-templates/>
  </fo:block>
</xsl:template>

<xsl:template match="//articleinfo/title" mode="hlavni">
  <fo:block font-family="Helvetica"
            font-size="20pt"
            font-weight="bold"
			font-style="italic"
			text-align="center"
			space-before="10em" space-after="5em">
    <xsl:apply-templates/>
  </fo:block>
  <fo:block font-family="Helvetica"
            font-size="13pt"
			font-weight="bold"
			text-align="center"
			space-before="0.25em" space-after="1em">
    <xsl:apply-templates select="//articleinfo//author" mode="titulek"/>
  </fo:block>  
  <fo:block font-family="Helvetica"
            font-size="10pt"
			text-align="center" space-after="5em">
    <xsl:apply-templates select="//othercredit/personname" mode="titulek"/>
  </fo:block>    
</xsl:template>

<xsl:template match="//othercredit/personname" mode="titulek">
  <fo:block text-align="center">
    <xsl:apply-templates/>
  </fo:block>
</xsl:template>

<xsl:template match="//articleinfo//author" mode="titulek">
  <fo:block text-align="center">
    <xsl:apply-templates/>
  </fo:block>
</xsl:template>


<xsl:template match="//author/email">
  <fo:block>
    e-mail:
    <xsl:apply-templates/>
  </fo:block>
</xsl:template>

<xsl:template match="//abstract/para" mode="hlavni">
  <fo:block font-style="italic" font-size="12pt"
            text-align="justify" font-weight="bold" language="cs" 
            hyphenate="true" space-before="10em">
    Abstrakt:
  </fo:block>
  <fo:block space-before="5pt" font-style="italic" font-size="10pt"
            text-align="justify" font-weight="bold" language="cs" 
            hyphenate="true" space-after="10em">
    <xsl:apply-templates/>
  </fo:block>
</xsl:template>

<xsl:template match="imageobject/imagedata">
  <fo:block margin-bottom="5px" margin-top="5px">
    <fo:external-graphic src="url({@fileref})"
                         content-width="10cm"/>
  </fo:block>
</xsl:template>

<xsl:template match="//section/para">
  <fo:block font-style="normal" text-align="justify" language="cs"
  	        font-family="Helvetica" font-size="8pt">
    <xsl:apply-templates/>
  </fo:block>
</xsl:template>

<xsl:template match="classname">
  <fo:inline font-weight="bold" font-family="Arial">
    <xsl:apply-templates/>
  </fo:inline>
</xsl:template>


<xsl:template match="//section/title">
  <fo:block font-family="Helvetica" font-size="12pt"
            font-weight="bold" text-align="left"
			margin-bottom="5px" margin-top="15px">
    <xsl:apply-templates/>
  </fo:block>
</xsl:template>

<xsl:template match="//section">
  <fo:block margin-bottom="5px">
    <xsl:apply-templates/>
  </fo:block>
</xsl:template>

<xsl:template match="//footnote/para">
  <fo:footnote>
    <fo:inline vertical-align="super" font-size="8pt">
	  [<xsl:number/>]
	</fo:inline>
    <fo:footnote-body>
      <fo:block font-size="8pt">
	  	[<xsl:number/>] <xsl:apply-templates/>
	  </fo:block>
    </fo:footnote-body>
  </fo:footnote>
</xsl:template>

<xsl:template match="citation">
   <fo:footnote> 
	<fo:inline font-family="Arial" font-size="8pt">
	  [<xsl:apply-templates/>]
	</fo:inline>
    <fo:footnote-body>
      <fo:block>
	  	<xsl:apply-templates/>
	  </fo:block>
    </fo:footnote-body>
  </fo:footnote>
</xsl:template>

<xsl:template match="//itemizedlist">
    <fo:list-block
      space-before="0.25em" space-after="0.25em">
        <xsl:apply-templates/>
    </fo:list-block>
</xsl:template>

<xsl:template match="//itemizedlist/listitem">
    <fo:list-item space-after="0.5em">
        <fo:list-item-label start-indent="1em" end-indent="1em">
            <fo:block>
                <xsl:number/>.
				<xsl:apply-templates/>
            </fo:block>
        </fo:list-item-label>
        <fo:list-item-body>
        </fo:list-item-body>
    </fo:list-item>
</xsl:template>

<xsl:template match="//listitem/formalpara/title">
  <fo:inline font-weight="bold">
    <xsl:apply-templates/>
  </fo:inline>
</xsl:template>

<xsl:template match="//listitem/formalpara/para">
  <fo:inline font-style="italic">
    <xsl:apply-templates/>
  </fo:inline>
</xsl:template>

<xsl:template match="//bibliography">
  <fo:block font-family="Helvetica" font-size="12pt"
            font-weight="bold" text-align="left"
			margin-bottom="5px" margin-top="15px">
      Bibliografie:
  </fo:block>
  <xsl:apply-templates/>
</xsl:template>

<xsl:template match="//biblioentry">
  <fo:block>
    <xsl:apply-templates/>
  </fo:block>
</xsl:template>

<xsl:template match="//biblioentry/abbrev">
  <fo:inline>
    [<xsl:apply-templates/>]
  </fo:inline>
</xsl:template>

<xsl:template match="//biblioentry/authorgroup">
  <fo:inline>
    <xsl:apply-templates/>
  </fo:inline>
</xsl:template>

<xsl:template match="//biblioentry/title">  
  <fo:inline font-style="italic">
    <xsl:apply-templates/>
  </fo:inline>
</xsl:template>

<xsl:template match="//biblioentry/publisher">  
  <fo:inline>
    <xsl:apply-templates/>
  </fo:inline>
</xsl:template>


<xsl:template match="//articleinfo//author">
</xsl:template>

<xsl:template match="//othercredit/personname">
</xsl:template>

<xsl:template match="//othercredit/email">
</xsl:template>

<xsl:template match="//titleabbrev">
</xsl:template>

<xsl:template match="//pubdate">
</xsl:template>

<xsl:template match="//articleinfo/title">
</xsl:template>

<xsl:template match="//abstract/para">
</xsl:template>


</xsl:stylesheet>