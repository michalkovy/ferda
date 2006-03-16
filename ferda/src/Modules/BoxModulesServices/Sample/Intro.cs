using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.Sample
{
    /// <summary>
    /// <para>
    /// Tento článek popisuje postup, jakým lze relativně snadno přidat 
    /// vlastní krabičku do systému „Ferda“. Tento článek popisuje postup 
    /// práce ve vývojovém prostředí Microsoft Visual Studia 2005 za použití
    /// jazyka C# verze 2.0. Analogicky lze použít i jiná prostředí a jazyky
    /// podporované platformou .NET nebo ICE.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <b>Prostředky</b>
    /// <para>Potřebné nástroje
    /// <list type="bullet">
    /// <item>
    /// <term><see href="http://www.zeroc.com">Ice</see></term>
    /// <description>
    /// Na stroji, kde chcete pouštět Ferdu musí být Ice nainstalováno.
    /// Dále budeme při implementaci krabiček potřebovat utilitu <b>slice2cs</b>,
    /// která bude generovat C# kód z našich slice návrhů krabiček (viz. dále).
    /// V neposlední řadě budeme potřebovat při buildění projektu reference na 
    /// několik Ice knihoven (viz. dále).
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>Potřebné reference
    /// <list type="bullet">
    /// <item>
    /// <term>FerdaBase.dll</term>
    /// <description>
    /// <para>
    /// Tato knihovna obsahuje datové struktury a datové typy, se kterými 
    /// systém Ferda pracuje, ...
    /// </para>
    /// <para>
    /// Tato knihovna je distribuována spolu s Ferdou.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>FerdaServerBase.dll</term>
    /// <description>
    /// <para>
    /// Mimo jiné obsahuje třídu <see cref="T:Ferda.Modules.BoxModuleI"/>, která je 
    /// nezbytná pro implementaci krabičky ... implementuje totiž rozhraní pro komunikaci
    /// s ModulesManagerem a ProjectManagerem (viz. dokumentace k Architektuře Ferdy).
    /// Pro implementaci nových krabiček je proto nezbytná implementace interface 
    /// <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>. Tato knihovna obsahuje řadu pomocných 
    /// tříd o kterých budeme hovořit později.
    /// </para>
    /// <para>
    /// Tato knihovna je distribuována spolu s Ferdou.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>FerdaBoxInterfaces.dll</term>
    /// <description>
    /// <para>
    /// Obsahuje kód vygenerovaný ze slice návrhů krabiček distribuovaných s Ferdou 
    /// tj. tato knihovna je nezbytná pokud chcete volat funkce těchto krabiček.
    /// </para>
    /// <para>
    /// Tato knihovna je distribuována spolu s Ferdou.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>icecs.dll</term>
    /// <description>
    /// <para>
    /// Knihovna pro práci s Ice v jazyce C#.
    /// </para>
    /// <para>
    /// Tato knihovna je distribuována mj. s Ice.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>iceboxcs.dll</term>
    /// <description>
    /// <para>
    /// Píšete-li vlastní službu, v niž poběží Vámi vytvořené krabičky (postup viz. níže) 
    /// implementujete abstraktní třídu <see cref="T:Ferda.Modules.FerdaServiceI"/>, která 
    /// zase implementuje interface IceBox.Service - Ten je definován v iceboxcs.dll.
    /// </para>
    /// <para>
    /// Tato knihovna je distribuována mj. s Ice.
    /// </para>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <b>Implementace krabičky</b>
    /// <para>
    /// Na krabičku (někdy též BoxModule nebo jen Module) se můžeme dívat z dvou různých
    /// pohledů.
    /// <list type="number">
    /// <item>
    /// <term>Krabička z pohledu Project Manageru a Modules Manageru</term>
    /// <description>
    /// <para>
    /// Každá krabička musí implementovat řadu funkcí, které umožní Modules (někdy Project)
    /// Manageru zprostředkovávat různé informace/data a funkcionalitu Front-Endu (někdy jiným 
    /// krabičkám). Příkladem takových funkcí mohou být funkce pro získání seznamu a popisu 
    /// vlastností (property) a zásuvek (socket) nebo například labelu krabičky či nápovědy 
    /// ke krabičce atd. Zájemci se mohou podívat například na následující slice návrhy 
    /// <c>Ferda.Modules.BoxModule</c>, <c>Ferda.Modules.BoxModuleFactory</c> a 
    /// <c>Ferda.Modules.BoxModuleFactoryCreator</c>; případně přímo na dokumentaci k implementaci 
    /// těchto návrhů (FerdaServerBase.dll) <see cref="T:Ferda.Modules.BoxModuleI"/>, 
    /// <see cref="T:Ferda.Modules.BoxModuleFactoryI"/> a 
    /// <see cref="T:Ferda.Modules.BoxModuleFactoryCreatorI"/>.
    /// </para>
    /// <para>
    /// Bude zde popsán nejsnadnější a nejefektivnější způsob jak tutu funkcionalitu 
    /// implementovat. Jak již bylo uvedeno výše příslušné slice návrhy jsou již 
    /// implementovány v knihovně FerdaServerBase.dll, kde je také řada pomocných funkcí.
    /// Zbývá ovšem implemetnovat interface <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>.
    /// Ten od nás odstínil některé detaily implementace výše zmíněných slice návrhů.
    /// </para>
    /// <para>
    /// Interface <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/> můžete imlementovat 
    /// přímo, což je ovšem poněkud zdlouhavý proces, neboť je zde potřeba řešit například
    /// lokalizaci apod. Proto zde popíšeme daleko rychlejší způsob. Je připraveno několik
    /// tříd (rovněž ve FerdaServerBase.dll), pomocí nichž budete mnoha práce zproštěni.
    /// Budete muset pouze implementovat abstraktní třídu <see cref="T:Ferda.Modules.Boxes.BoxInfo"/>
    /// tj. asi 6 funkcí a bude-li to třeba přetížíte několik virtuálních funkcí:
    /// <list type="table">
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.GetPropertyObjectFromInterface(System.String,Ice.ObjectPrx)"/></term>
    /// <description>
    /// Implementujte, pokud používáte „nestadardní“ datový typ pro vlastnost/zásuvku. 
    /// Implicitně vrací null. TODO
    /// </description>
    /// </item>
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.GetReadOnlyPropertyValue(System.String,Ferda.Modules.BoxModuleI)"/></term>
    /// <description>
    /// Implementujte, pokud Vaše krabička má nějakou readonly vlastnost 
    /// (zde implementujete její výpočet). Není-li implementována přestože 
    /// je potřeba, je vyvolána výjimka.
    /// </description>
    /// </item>
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.IsPropertySet(System.String,Ferda.Modules.PropertyValue)"/></term>
    /// <description>
    /// Implementujte, pokud používáte „nestadardní“ datový typ (který nepoužíváte 
    /// pouze jako readonly vlastnost). Umožní (nejen) Front-Endu rozpoznat, kdy 
    /// je vlastnost již nastavena. Implicitně vrací <c>true</c>.
    /// </description>
    /// </item>
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.GetDynamicHelpItems(System.String[],Ferda.Modules.BoxModuleI)"/></term>
    /// <description>
    /// Tato funkce implicitně vrací všechny definované položky dynamické (v tuto 
    /// chvíli ještě nedynamické) nápovědy. Chcete-li, aby se obsah nápovedy 
    /// dynamicky měnil podle vnitřního stavu krabičky, přetěžte tuto funkci.
    /// </description>
    /// </item>
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.RunAction(System.String,Ferda.Modules.BoxModuleI)"/></term>
    /// <description>
    /// Implementujte, pokud má Vaše krabička nějaké 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Actions">akce</see>
    /// (které uživatel explicitně volá z Front-Endu). Není-li implementována 
    /// přestože je potřeba, je vyvolána vyvolá výjimka.
    /// </description>
    /// </item>
    /// </list>
    /// K abstraktním i výše uvedeným virtuálním funkcím jsou v programátorské nápovědě 
    /// uvedeny příklady, jak je implementovat, proto je zde nebudu dále komentovat.
    /// Nezbytným krokem při implementaci interfacu <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>
    /// s pomocí abstraktní třídy <see cref="T:Ferda.Modules.Boxes.BoxInfo"/> je vytvoření
    /// konfiguračních a lokalizačních XML souborů a jejich správné umístění v adresářové
    /// struktuře Ferdy (TODO zdokumentovat).
    /// </para>
    /// <b>Konfigurační a lokalizační XML soubory</b>
    /// <para>
    /// Konfigurační a lokalizační soubory vytvářejte podle příslušných XSD schémat 
    /// a nápovědy:
    /// <list type="bullet">
    /// <listheader>
    /// <term>dokumentace</term>
    /// <description>XSD schémata</description>
    /// </listheader>
    /// <item>
    /// <term><see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/></term>
    /// <description>src/Modules/Core/Boxes/box.xsd</description>
    /// </item>
    /// <item>
    /// <term><see cref="T:Ferda.Modules.Boxes.Serializer.Localization.BoxLocalization"/></term>
    /// <description>src/Modules/Core/Boxes/boxLocalization.xsd</description>
    /// </item>
    /// </list>
    /// Při konstrukci objektu <see cref="T:Ferda.Modules.Boxes.BoxInfo"/> se tyto
    /// XML kofigurační resp. lokalizační soubory deserealizují. Pomocí těchto dat 
    /// je imlementována řada funkcí z <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>Krabička z pohledu ostatních krabiček</term>
    /// <description>
    /// <para>
    /// Krabička z poledu ostatních krabiček poskytuje určitou funkci resp. funkce (vlastní 
    /// funkcionalita krabičky). V tuto chvíli je vhodné se podívat na dokumentaci 
    /// k formalizmu principu zapojování krabiček do zásuvek (TODO Michal). Na tomto místě uvedu jen 
    /// velmi stručné a nepřesné (avšak v tuto chvili snad postačující) vysvětlení:
    /// každá krabička implementuje určité rozhraní (tj funkci/množinu funkcí) resp. 
    /// několik rozhraní. Tyto rozhraní jsou (vnitřním mechanismem Ice) pojmenovány 
    /// a zásuvky (sokety) pak uvedou 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.BoxType.FunctionIceId">
    /// seznam jmen rozhraní</see>, které akceptují.
    /// </para>
    /// <para>
    /// Uveďme na tomto místě jednoduchý příklad toho, jak implementovat 
    /// funkcionalitu krabičky z pohledu ostatních krabiček. Nejprve je třeba 
    /// vytvořit slice návrh (rozhraní) a definovat tak funkci/množinu funkcí, 
    /// které krabička implementuje. V následujícím příkladu definujeme dvě rozhaní.
    /// Naše krabička, která pak bude implementovat rozhraní 
    /// <c>MyBoxModule.MyBoxModuleFunctions</c> bude moci být zasunuta do zásuvek 
    /// akceptujících alespoň jeden z následujícíh (pojmenovanych) rozhraní: 
    /// <c>::MyBoxModule::MyBoxModuleFunctionsCore</c>
    /// nebo <c>::MyBoxModule::MyBoxModuleFunctions</c>. 
    /// </para>
    /// <code>
    /// //soubor MyBoxModule.ice
    /// module MyBoxModule
    /// {
    ///     interface MyBoxModuleFunctionsCore
    ///     {
    ///         // tato funkce vráti řetězec "Hello World!"
    ///         nonmutating string HelloWorld();
    ///     };
    /// 
    /// 	interface MyBoxModuleFunctions extends MyBoxModule::MyBoxModuleFunctionsCore
    /// 	{
    ///         // tato funkce vrátí vstupní řetězec (echoString)
    ///         nonmutating string Echo(string echoString);
    /// 
    /// 		/* ... další funkce ... */
    /// 	};
    /// };
    /// </code>
    /// <para>
    /// Nyní pomocí utility slie2cs vygenerujeme z našeho slice návrhu 
    /// (soubor MyBoxModule.ice) C# kód (jehož obsah nás příliš/vůbec 
    /// nemusí zajímat). Postup generování je dobře popsán k dokumentaci 
    /// k Ice, zde uvedeme jen nástin a jednoduchý příklad:
    /// Nechť cesta ke adresáři Ice/bin (např. c:\Ice-3.0.0\bin\") je 
    /// v cestě (systémová proměnná PATH) pak následujícím příkazem 
    /// (z příkazové řádky resp. v shellu) vygenerujeme C# kód z našich
    /// slice návrhů.
    /// <code>
    /// $ slice2cs [options] slice-files
    /// Options:
    /// - Argument -IDIR, kde DIR je cesta k *.ice souborům tj. slice návrhům, 
    ///   na které se v našem slice návru odkazujeme pomocí direktivy #include
	/// - Argument --output-dir je cesta do adresáře, kam se výsledné C# (*.cs) 
    ///   soubor/y vygenerují.
    /// Slice-Files:
	/// - Seznam souborů ke zpracování (vygeneruje *.cs ze zadaných *.ice)
    /// 
    /// Více z příkazové řádky pomocí $ slice2cs --help
    /// </code>
    /// V našem případě tedy:
    /// <code>
    /// $ mkdir generated
    /// $ slice2cs --output-dir generated MyBoxModule.ice
    /// </code>
    /// </para>
    /// <para>
    /// Jak již bylo uvedeno výše naše krabička bude implementovat rozhraní 
    /// <c>MyBoxModule.MyBoxModuleFunctions</c> (chcete-li 
    /// <c>::MyBoxModule::MyBoxModuleFunctions</c>), toho dosáhneme tak, že 
    /// vytvoříme novu třídu, která bude implementovat abstraktní (vygenerovanou 
    /// třídu) <c>MyBoxModuleFunctionsDisp_</c> (jmenuje se stejne jako námi 
    /// definované rozhraní ve slice návrhu + řetězec "<c>Disp_</c>").
    /// Povšimněte si prosím, že třída implementující funkce krabičky musí 
    /// rovněž implementovat rozhraní <see cref="T:Ferda.Modules.IFunctions"/>
    /// (implementuje se vždy stejně). Zde jen naokraj uvedu, že z modulu 
    /// (ve slice návrhu) <c>MyBoxModule</c> byl vygenerován stejnojmenný 
    /// jmenný prostor (více v dokumentaci k Ice).
    /// </para>
    /// <code>
    /// namespace MyBoxModule
    /// {
    ///		class MyBoxModuleFunctionsI : MyBoxModuleFunctionsDisp_, Ferda.Modules.IFunctions
    ///		{
    ///		    protected Ferda.Modules.BoxModuleI boxModule;
    ///		    protected Ferda.Modules.Boxes.IBoxInfo boxInfo;
    /// 
    ///         // each functions object has to implement Ferda.Modules.IFunctions interface
    ///		    #region IFunctions Members
    ///		    void Ferda.Modules.IFunctions.setBoxModuleInfo(Ferda.Modules.BoxModuleI boxModule, Ferda.Modules.Boxes.IBoxInfo boxInfo)
    ///		    {
    ///	    	    this.boxModule = boxModule;
    ///	    	    this.boxInfo = boxInfo;
    ///	    	}
    ///		    #endregion
    /// 
    /// 		//this implements HelloWorld() method specified in slice design
    ///         public override string HelloWorld(Ice.Current __current)
    /// 		{
    /// 			return "Hello World!";
    /// 		}
    /// 
    /// 		//this implements Echo(string echoString) method specified in slice design
    ///         public override string Echo(string echoString, Ice.Current __current)
    /// 		{
    /// 			return echoString;
    /// 		}
    /// 
    ///			/* ... další funkce ... */
    ///		}
    /// }
    /// </code>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <b>Registrace krabičky</b>
    /// TODO Michal ... + zdokumentovat tu Service svini :-)
    /// <para>
    /// Vytvořte třídu, která implementuje abstraktní třídu 
    /// <see cref="T:Ferda.Modules.FerdaServiceI"/>. K tomu 
    /// stačí implementovat (triviálně) jednu její metodu 
    /// <see cref="M:Ferda.Modules.FerdaServiceI.registerBoxes()"/>.
    /// </para>
    /// <code>
    /// public class Service : Ferda.Modules.FerdaServiceI
    /// {
    ///     protected override void registerBoxes()
    ///     {
    ///         // the first parameter is identifier which has to be unique
    ///         // accross all services (in the IceGrid)
    ///         this.registerBox(
    ///             "BodyMassIndexSampleFactoryCreator", 
    ///             new Sample.BodyMassIndex.BodyMassIndexBoxInfo()
    ///             );
    ///         // if more boxes should be provided by this service, register them here ...
    ///     }
    /// }
    /// </code>
    /// <para>
    /// Nyní je potřeba ... tj conf/db/application.xml, odinstalovat + nainstalovat (refresh)
    /// </para>
    /// </remarks>
    /// <seealso href="http://www.microsoft.com/net/">Microsoft .NET Framework</seealso>
    /// <seealso href="http://msdn.microsoft.com/library/en-us/cscon/html/vcoriCStartPage.asp">Microsoft C# programming language</seealso>
    /// <seealso href="http://msdn.microsoft.com/vstudio/">Microsoft Visual Studio 2005</seealso>
    /// <seealso href="http://www.w3.org/XML/">XML</seealso>
    /// <seealso href="http://www.w3.org/XML/Schema">XML Schema (XSD)</seealso>
    /// <seealso href="http://www.nunit.org/">NUnit</seealso>
    /// <seealso href="http://ndoc.sourceforge.net/">NDoc</seealso>
    public class Intro
    {
    }
}
