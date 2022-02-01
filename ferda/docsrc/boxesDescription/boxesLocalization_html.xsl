<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
    xmlns:ns="http://ferda.is-a-geek.net">
    
    <xsl:output name="html"  encoding="UTF-8" doctype-system="http://www.w3.org/TR/html4/strict.dtd" doctype-public="-//W3C//DTD HTML 4.01//EN"/>
    
    <xsl:template match="/">
     
        <html>
            <head>
                <title>Lokalizace krabiček</title>
                <link rel="stylesheet" type="text/css" href="styl.css"/> 
                
            </head>
            
            <body>
               <div id="base"> 
                   <h1>Obsah:</h1>
                         <xsl:apply-templates/>
                     
                </div>      
            </body>
            
        </html>

    </xsl:template>
    
            <xsl:template match="ns:BoxesLocalization"> 


                 <xsl:apply-templates/>
                
                <xsl:apply-templates mode="detail"/>
          
            </xsl:template>
    
            <xsl:template match="ns:BoxLocalization">
           
                <p><a href="#{generate-id(ns:Identifier/text())}"><xsl:value-of select="ns:Identifier"/></a></p>

            </xsl:template>
    
   
    
    <xsl:template match="ns:BoxLocalization" mode="detail">
        
        <xsl:apply-templates mode="detail" />

    </xsl:template>
     
    
    
    <xsl:template match="ns:Identifier" mode="detail">
      
        <h1><a name="{generate-id(text())}"></a><xsl:value-of select="text()"/></h1>
    
    </xsl:template>    
    
    
    <xsl:template match="ns:Label" mode="detail">
    
        <h3>Label:  </h3>
        
        <xsl:apply-templates mode="detail"/>
    
    </xsl:template>
         
    <xsl:template match="ns:Hint" mode="detail">
 
        <h3>Hint: </h3>
        
        <xsl:apply-templates mode="detail"/>
   
    </xsl:template>
    
    <xsl:template match="ns:Categories" mode="detail">
        
        <h3>Categories (<xsl:value-of select="count(ns:Category)"/>):  </h3>
        
        <table border="1">
            <th>Name</th>
            <th>Label</th>
            <xsl:apply-templates mode="detail"/>
        </table>
        
    </xsl:template>
    
    
    <xsl:template match="ns:Category" mode="detail">
       
        <tr>
           <td class="prvni"><b><xsl:value-of select="ns:Name"/></b></td>
           <td><xsl:value-of  select="ns:Label"/></td>
       </tr>  
        
    </xsl:template>
    
    <xsl:template match="ns:Actions" mode="detail">
        
        <h3>Actions (<xsl:value-of select="count(ns:Action)"/>):  </h3> 
        <table border="1">
               <th>Name</th>
               <th>Label</th>
               <th>Hint</th>
               <xsl:apply-templates mode="detail"/>
        </table>
        
    </xsl:template>
    
    <xsl:template match="ns:Action" mode="detail">
       
        <tr>
             <td class="prvni"><b><xsl:value-of select="ns:Name"/></b></td> 
             <td><xsl:value-of  select="ns:Label"/></td>
             <td><xsl:value-of  select="ns:Hint"/></td>
        </tr>
        
    </xsl:template>
    
    
    
    <xsl:template match="ns:Sockets" mode="detail">
        
        <h3>Sockets (<xsl:value-of select="count(ns:Socket)"/>):</h3> 
        <table border="1">
            <th>Name</th>
            <th>Label</th>
            <th>Hint</th>
            <th>SelectOptions</th>
            <xsl:apply-templates mode="detail"/>
        </table>
        
    </xsl:template>
    
    <xsl:template match="ns:Socket" mode="detail">

        <tr>
            <td class="prvni"><b><xsl:value-of select="ns:Name"/></b></td>
            <td><xsl:value-of  select="ns:Label"/></td>
            <td><xsl:value-of  select="ns:Hint"/></td>
            <td><xsl:apply-templates select="ns:SelectOptions" mode="detail"/></td>
        </tr>     
    
    </xsl:template>
       
    <xsl:template match="ns:SelectOption" mode="detail">
        
        <xsl:value-of select="ns:Name"/><xsl:text> ,</xsl:text>
        
    </xsl:template> 

    <xsl:template match="ns:PropertyCategories" mode="detail">
        
        <h3>Property categories (<xsl:value-of select="count(ns:PropertyCategory)"/>):  </h3>
        
        <table border="1">
            <th>Name</th>
            <th>Label</th>
            <xsl:apply-templates mode="detail"/>
        </table>        
        
    </xsl:template>    



    <xsl:template match="ns:PropertyCategory" mode="detail">
      
        <tr>
            <td class="prvni"><b><xsl:value-of select="ns:Name"/></b></td>
            <td><xsl:value-of  select="ns:Label"/></td>
        </tr>  


    </xsl:template>
    
    <xsl:template match="ns:ModulesAskingForCreationSeq" mode="detail">
    
        <h3>ModulesAskingForCreationSeq (<xsl:value-of select="count(ns:ModulesAskingForCreation)"/>): </h3>
        <table border="1">
            <th>Name</th>
            <th>Label</th>
            <th>Hint</th>
            <xsl:apply-templates mode="detail"/>
        </table>
    
    </xsl:template>
    
    <xsl:template match="ns:ModulesAskingForCreation" mode="detail">
         
        <tr>
            <td class="prvni"><b><xsl:value-of select="ns:Name"/></b></td>
            <td><xsl:value-of  select="ns:Label"/></td>
            <td><xsl:value-of  select="ns:Hint"/></td>            
        </tr>    
        
    </xsl:template>
  
  
    <xsl:template match="ns:Phrases" mode="detail">
        
        <h3>Phrases (<xsl:value-of select="count(ns:Phrase)"/>): </h3>
        <table border="1">
            <th>PhraseIdentifier</th>
            <th>PhraseText</th>
            <xsl:apply-templates mode="detail"/>
        </table>
        
        
     </xsl:template>   
        
  
  
    <xsl:template match="ns:Phrase" mode="detail">

        <tr>
            <td class="prvni"><b><xsl:value-of select="ns:PhraseIdentifier"/></b></td>
            <td><xsl:value-of  select="ns:PhraseText"/></td>
        </tr>  


    </xsl:template>
    

</xsl:stylesheet>
