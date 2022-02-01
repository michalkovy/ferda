<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0"  xmlns:fo="http://www.w3.org/1999/XSL/Format"
    xmlns:ns="http://ferda.is-a-geek.net">

    <xsl:template match="/">
        <fo:root font-family="Helvetica,Arial" font-size="115%" language="cs">
            <fo:layout-master-set>
                <fo:simple-page-master master-name="Boxes-Uvodni" 
                    page-height="210mm" 
                    page-width="297mm" 
                    margin-top="10mm" 
                    margin-right="15mm" 
                    margin-bottom="10mm" 
                    margin-left="15mm">
                    
                    <fo:region-body/>        
                    <fo:region-before extent="0mm"/>
                    <fo:region-after extent="0mm"/>
                    <fo:region-end extent="0mm"/>
                </fo:simple-page-master>
                
                
                <fo:simple-page-master master-name="Boxes-Obsah" 
                    page-height="210mm" 
                    page-width="297mm" 
                    margin-top="5mm" 
                    margin-right="18mm" 
                    margin-bottom="10mm" 
                    margin-left="18mm">
                   
                    <fo:region-body/>        
                    <fo:region-before extent="0mm"/>
                    <fo:region-after extent="0mm"/>
                    <fo:region-end extent="0mm"/>
                </fo:simple-page-master>
         
           
                  <fo:simple-page-master master-name="Boxes-Detail" 
                        page-height="210mm" 
                        page-width="297mm" 
                        margin-top="5mm" 
                        margin-right="15mm" 
                        margin-bottom="10mm" 
                        margin-left="15mm">
                        
                        <fo:region-body/>        
                        <fo:region-before extent="0mm"/>
                        <fo:region-after extent="0mm"/>
                        <fo:region-end extent="0mm"/>
                    </fo:simple-page-master>
            </fo:layout-master-set>
            
            <fo:page-sequence master-reference="Boxes-Uvodni">
                
                <fo:flow flow-name="xsl-region-body">
                    <fo:block-container >
                        <fo:block font-size="230%" margin-top="65pt" text-align="center">
                             <xsl:text>Lokalizační soubor krabiček systému Ferda</xsl:text>
                        </fo:block>
                        <fo:block color="#bebebe" margin-top="20pt" text-align="center">
                              <xsl:text>PDF verze</xsl:text>
                        </fo:block>
                    </fo:block-container>
                </fo:flow>   
              
            </fo:page-sequence>
            
            
            <fo:page-sequence master-reference="Boxes-Obsah">

             <fo:flow flow-name="xsl-region-body">
                
              <fo:block font-size="75%" margin-top="35pt">
                 <fo:block font-size="195%" margin-bottom="5pt" >Obsah:</fo:block>

                        <xsl:apply-templates select="ns:BoxesLocalization/ns:BoxLocalization"/>

                    </fo:block>
                </fo:flow>   
                
            </fo:page-sequence>
        
        
         <fo:page-sequence master-reference="Boxes-Detail">
             
             <fo:static-content flow-name="xsl-region-after">
                 <fo:block font-size="60%" text-align="center" color="black">
                     <xsl:text>Strana </xsl:text><fo:page-number/><xsl:text> z </xsl:text><fo:page-number-citation ref-id="" />
                 </fo:block>  
             </fo:static-content>
             
             <fo:static-content flow-name="xsl-region-before">
                     <fo:block text-align-last="right"  color="black"  font-size="60%" margin-top="10pt" id="{generate-id(text())}">   
                         <xsl:text>Boxes Localization Ferda</xsl:text>
                     </fo:block>
             </fo:static-content>
             
             <fo:flow flow-name="xsl-region-body">
            
                 <fo:block font-size="60%" margin-top="20pt">
                     
                     <xsl:apply-templates select="ns:BoxesLocalization/ns:BoxLocalization" mode="detail"/>
                     
                 </fo:block>
             </fo:flow>   

            </fo:page-sequence>


        </fo:root>
      
    </xsl:template>


    <xsl:template match="ns:BoxLocalization">

  
             <fo:block text-align-last="justify">
               <fo:basic-link internal-destination="{generate-id(ns:Identifier/text())}" ><xsl:value-of select="ns:Identifier"/> 
                   <fo:leader leader-pattern="dots"/>
                   <fo:page-number-citation ref-id="{generate-id(ns:Identifier/text())}"/>
               </fo:basic-link>
         </fo:block>
        
     
  
    </xsl:template>
    

    <xsl:template match="ns:BoxLocalization" mode="detail"> 

     <fo:block  keep-together="always" break-after="page" id="{generate-id(ns:Identifier)}">
        <xsl:apply-templates mode="detail"/>
   </fo:block>
        
    </xsl:template>
    
    
    <xsl:template match="ns:Identifier" mode="detail">
        
        <fo:block  color="black" font-size="220%" font-weight="bold" id="{generate-id(text())}" margin-top="10pt" margin-bottom="5pt">
        
                 <xsl:value-of select="text()"/>
        
        </fo:block>
        
    </xsl:template>    
    


    <xsl:template match="ns:Label" mode="detail">
        
        <fo:block margin-top="3pt" font-weight="bold" font-size="140%">Label:  <fo:inline font-weight="normal" font-size="85%"><xsl:apply-templates mode="detail"/></fo:inline></fo:block>
        
    </xsl:template>
    
    
    <xsl:template match="ns:Hint" mode="detail">
        
        <fo:block margin-top="3pt" font-weight="bold" font-size="140%">Hint:  <fo:inline font-weight="normal" font-size="85%"><xsl:apply-templates mode="detail"/></fo:inline></fo:block>
        
    </xsl:template>
    
    
    <xsl:template match="ns:Categories" mode="detail">
        
        <fo:block margin-top="3pt" font-weight="bold" font-size="140%">Categories <fo:inline font-weight="normal" font-size="85%">(<xsl:value-of select="count(ns:Category)"/>):</fo:inline></fo:block>
      <fo:table background-color="white" border="2pt" border-style="solid"  display-align="center" margin-top="5pt">
        <fo:table-header>
            <fo:table-row  text-align="center" color="black" background-color="#bebebe" border-color="black" font-weight="bold" border-style="solid" border-width="1pt">
                <fo:table-cell padding="2pt" border-style="solid" border-width="0.5pt" border-color="black">
                    <fo:block>Name</fo:block>
                </fo:table-cell>
                <fo:table-cell padding="2pt" border-style="solid" border-width="0.5pt" border-color="black">
                    <fo:block>Label</fo:block>
                </fo:table-cell>
            </fo:table-row>
        </fo:table-header>
        <fo:table-body>
   
            <xsl:apply-templates mode="detail"/>
            
        </fo:table-body>
        </fo:table>
    
    </xsl:template>
    
    
    <xsl:template match="ns:Category" mode="detail">
       
        <fo:table-row>
            <fo:table-cell padding="2pt" border-width="0.5pt" border-style="solid" background-color="#ededed" border-color="#bebebe">
                
                <fo:block><xsl:value-of select="ns:Name"/>  </fo:block>
                
            </fo:table-cell>
            <fo:table-cell padding="2pt"  border-width="0.5pt" border-style="solid" border-color="#bebebe">
                
                <fo:block><xsl:value-of select="ns:Label"/></fo:block>
                
            </fo:table-cell>
        </fo:table-row>

    </xsl:template>
    
    
    <xsl:template match="ns:Actions" mode="detail">
        <fo:block margin-top="3pt" font-weight="bold" font-size="140%">Actions <fo:inline font-weight="normal" font-size="85%">(<xsl:value-of select="count(ns:Action)"/>): </fo:inline></fo:block> 
     
        <fo:table background-color="white" border="2pt" border-style="solid"  display-align="center" margin-top="5pt">
            <fo:table-header>
                <fo:table-row border-style="solid" border-width="1pt"  text-align="center" color="black" border-color="black" background-color="#bebebe" font-weight="bold">
                    <fo:table-cell padding="2pt" border-style="solid"  border-width="0.5pt" border-color="black">
                        <fo:block>Name</fo:block>
                    </fo:table-cell>
                    <fo:table-cell padding="2pt" border-style="solid"  border-width="0.5pt" border-color="black">
                        <fo:block>Label</fo:block>
                    </fo:table-cell>
                    <fo:table-cell padding="2pt" border-style="solid"  border-width="0.5pt" border-color="black">
                        <fo:block>Hint</fo:block>
                    </fo:table-cell>
                </fo:table-row>
            </fo:table-header>
            <fo:table-body>
                
                <xsl:apply-templates mode="detail"/>
                
            </fo:table-body>
        </fo:table>
        
    </xsl:template>
    
    
    <xsl:template match="ns:Action" mode="detail">
        
        <fo:table-row>
            <fo:table-cell padding="2pt"  border-style="solid" border-width="0.5pt" background-color="#ededed" border-color="#bebebe">
                
                <fo:block><xsl:value-of select="ns:Name"/></fo:block>
                
            </fo:table-cell>
            <fo:table-cell padding="2pt" border-style="solid" border-width="0.5pt" border-color="#bebebe">
                
                <fo:block><xsl:value-of select="ns:Label"/></fo:block>
                
            </fo:table-cell>
            <fo:table-cell padding="2pt" border-style="solid" border-width="0.5pt" border-color="#bebebe">
                
                <fo:block><xsl:value-of select="ns:Hint"/></fo:block>
                
            </fo:table-cell>
        </fo:table-row>  
        
    </xsl:template>


   <xsl:template match="ns:Sockets" mode="detail">
       <fo:block margin-top="3pt" font-weight="bold" font-size="140%">Sockets <fo:inline font-weight="normal" font-size="85%">(<xsl:value-of select="count(ns:Socket)"/>): </fo:inline></fo:block>
       
       
       <fo:table background-color="white" border="2pt" border-style="solid"  display-align="center" margin-top="5pt">
           <fo:table-header>
               <fo:table-row border-style="solid" border-width="1pt"  text-align="center" color="black" border-color="black" background-color="#bebebe" font-weight="bold">
                   <fo:table-cell padding="2pt" border-style="solid"  border-width="0.5pt" border-color="black">
                       <fo:block>Name</fo:block>
                   </fo:table-cell>
                   <fo:table-cell padding="2pt" border-style="solid" border-width="0.5pt" border-color="black">
                       <fo:block>Label</fo:block>
                   </fo:table-cell>
                   <fo:table-cell padding="2pt" border-style="solid"  border-width="0.5pt" border-color="black">
                       <fo:block>Hint</fo:block>
                   </fo:table-cell>
                   <fo:table-cell padding="2pt" border-style="solid"  border-width="0.5pt" border-color="black">
                       <fo:block>SelectOptions</fo:block>
                   </fo:table-cell>
               </fo:table-row>
           </fo:table-header>
           <fo:table-body>
   
               <xsl:apply-templates mode="detail"/>
  
           </fo:table-body>
           </fo:table>
  
    </xsl:template>
    
    
    <xsl:template match="ns:Socket" mode="detail">
        
        
   
            <fo:table-row>
                <fo:table-cell padding="2pt"  border-style="solid" border-width="0.5pt" background-color="#ededed"  border-color="#bebebe">
                    
                    <fo:block><xsl:value-of select="ns:Name"/>  </fo:block>
                    
                </fo:table-cell>
                <fo:table-cell padding="2pt" border-style="solid" border-width="0.5pt" border-color="#bebebe">
                    
                    <fo:block><xsl:value-of select="ns:Label"/></fo:block>
                    
                </fo:table-cell>
                <fo:table-cell padding="2pt" border-style="solid" border-width="0.5pt" border-color="#bebebe">
                    
                    <fo:block><xsl:value-of select="ns:Hint"/></fo:block>
                    
                </fo:table-cell>
                <fo:table-cell padding="2pt" border-style="solid" border-width="0.5pt" border-color="#bebebe">
                   
                    <fo:block><xsl:apply-templates select="ns:SelectOptions" mode="detail"/></fo:block>
                    
                </fo:table-cell>
            </fo:table-row>  
    
    </xsl:template>
    
    
    <xsl:template match="ns:SelectOption" mode="detail">
        
        <xsl:value-of select="ns:Name"/><xsl:text> ,</xsl:text>
        
    </xsl:template> 


    <xsl:template match="ns:PropertyCategories" mode="detail">
        
        <fo:block margin-top="3pt" font-weight="bold" font-size="140%">PropertyCategories <fo:inline font-weight="normal" font-size="85%">(<xsl:value-of select="count(ns:PropertyCategory)"/>): </fo:inline></fo:block>
        
        <fo:table background-color="white" border="2pt" border-style="solid"  display-align="center" margin-top="5pt">
            <fo:table-header>
                <fo:table-row border-style="solid" border-width="1pt"  text-align="center" color="black" border-color="black" background-color="#bebebe" font-weight="bold">
                    <fo:table-cell padding="2pt" border-style="solid"  border-width="0.5pt" border-color="black">
                        <fo:block>Name</fo:block>
                    </fo:table-cell>
                    <fo:table-cell padding="2pt" border-style="solid"  border-width="0.5pt" border-color="black">
                        <fo:block>Label</fo:block>
                    </fo:table-cell>
                </fo:table-row>
            </fo:table-header>
            <fo:table-body>
                
                <xsl:apply-templates mode="detail"/>
                
            </fo:table-body>
        </fo:table>
             
    </xsl:template>    
    
    
    
    <xsl:template match="ns:PropertyCategory" mode="detail">
        
        <fo:table-row>
            <fo:table-cell padding="2pt"  border-style="solid" border-width="0.5pt" background-color="#ededed"  border-color="#bebebe">
                
                <fo:block><xsl:value-of select="ns:Name"/>  </fo:block>
                
            </fo:table-cell>
            <fo:table-cell padding="2pt" border-style="solid" border-width="0.5pt"  border-color="#bebebe">
                
                <fo:block><xsl:value-of select="ns:Label"/></fo:block>
                
            </fo:table-cell>
        </fo:table-row>
        
        
    </xsl:template>
    
    
   <xsl:template match="ns:ModulesAskingForCreationSeq" mode="detail">
        
       <fo:block margin-top="3pt" font-weight="bold" font-size="140%">ModulesAskingForCreationSeq <fo:inline font-weight="normal" font-size="85%">(<xsl:value-of select="count(ns:ModulesAskingForCreation)"/>): </fo:inline></fo:block>
       
       <fo:table background-color="white" border="2pt" border-style="solid"  display-align="center" margin-top="5pt">
           <fo:table-header>
               <fo:table-row border-style="solid" border-width="1pt"  text-align="center" color="black" border-color="black" background-color="#bebebe" font-weight="bold">
                   <fo:table-cell padding="2pt" border-style="solid"  border-width="0.5pt" border-color="black">
                       <fo:block>Name</fo:block>
                   </fo:table-cell>
                   <fo:table-cell padding="2pt" border-style="solid"  border-width="0.5pt" border-color="black">
                       <fo:block>Label</fo:block>
                   </fo:table-cell>
                   <fo:table-cell padding="2pt" border-style="solid"  border-width="0.5pt" border-color="black">
                       <fo:block>Hint</fo:block>
                   </fo:table-cell>
               </fo:table-row>
           </fo:table-header>
           <fo:table-body>
               
               <xsl:apply-templates mode="detail"/>
               
           </fo:table-body>
       </fo:table>
        
        
   </xsl:template>
    
    
    <xsl:template match="ns:ModulesAskingForCreation" mode="detail">
        
        <fo:table-row>
            <fo:table-cell padding="2pt"  border-style="solid" border-width="0.5pt" background-color="#ededed"  border-color="#bebebe">
                
                <fo:block><xsl:value-of select="ns:Name"/></fo:block>
                
            </fo:table-cell>
            <fo:table-cell padding="2pt" border-style="solid" border-width="0.5pt" border-color="#bebebe">
                
                <fo:block><xsl:value-of select="ns:Label"/></fo:block>
                
            </fo:table-cell>
            <fo:table-cell padding="2pt" border-style="solid" border-width="0.5pt" border-color="#bebebe">
                
                <fo:block><xsl:value-of select="ns:Hint"/></fo:block>
                
            </fo:table-cell>
        </fo:table-row>  
        
    </xsl:template>
    
    
    <xsl:template match="ns:Phrases" mode="detail">
        
        <fo:block margin-top="3pt" font-weight="bold" font-size="140%">Phrases <fo:inline font-weight="normal" font-size="85%">(<xsl:value-of select="count(ns:Phrase)"/>): </fo:inline></fo:block>
        
        <fo:table background-color="white" border="2pt" border-style="solid"  display-align="center" margin-top="5pt">
            <fo:table-header>
                <fo:table-row border-style="solid" border-width="1pt"  text-align="center" color="black" border-color="black" background-color="#bebebe" font-weight="bold">
                    <fo:table-cell padding="2pt" border-style="solid"  border-width="0.5pt" border-color="black">
                        <fo:block>PhraseIdentifier</fo:block>
                    </fo:table-cell>
                    <fo:table-cell padding="2pt" border-style="solid"  border-width="0.5pt" border-color="black">
                        <fo:block>PhraseText</fo:block>
                    </fo:table-cell>
                </fo:table-row>
            </fo:table-header>
            <fo:table-body>
                
                <xsl:apply-templates mode="detail"/>
                
            </fo:table-body>
        </fo:table>
     
    </xsl:template>   
    
    
    
    <xsl:template match="ns:Phrase" mode="detail">
       
        <fo:table-row>
            <fo:table-cell padding="2pt" border-style="solid" border-width="0.5pt" background-color="#ededed"  border-color="#bebebe">
                
                <fo:block><xsl:value-of select="ns:PhraseIdentifier"/></fo:block>
                
            </fo:table-cell>
            <fo:table-cell padding="2pt" border-style="solid" border-width="0.5pt" border-color="#bebebe">
                
                <fo:block><xsl:value-of select="ns:PhraseText"/></fo:block>
                
            </fo:table-cell>
        </fo:table-row>  
       
        </xsl:template>
    

</xsl:stylesheet>
