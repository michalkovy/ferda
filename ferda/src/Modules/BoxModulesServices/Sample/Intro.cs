using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.Sample
{
    /// <summary>
    /// <para>
    /// Tento èlánek popisuje postup, jakım lze relativnì snadno pøidat 
    /// vlastní krabièku do systému „Ferda“. Tento èlánek popisuje postup 
    /// práce ve vıvojovém prostøedí Microsoft Visual Studia 2005 za pouití
    /// jazyka C# verze 2.0. Analogicky lze pouít i jiná prostøedí a jazyky
    /// podporované platformou .NET nebo ICE.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <b>Prostøedky</b>
    /// <para>Potøebné nástroje
    /// <list type="bullet">
    /// <item>
    /// <term><see href="http://www.zeroc.com">Ice</see></term>
    /// <description>
    /// Na stroji, kde chcete pouštìt Ferdu musí bıt Ice nainstalováno.
    /// Dále budeme pøi implementaci krabièek potøebovat utilitu <b>slice2cs</b>,
    /// která bude generovat C# kód z našich slice návrhù krabièek (viz. dále).
    /// V neposlední øadì budeme potøebovat pøi buildìní projektu reference na 
    /// nìkolik Ice knihoven (viz. dále).
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>Potøebné reference
    /// <list type="bullet">
    /// <item>
    /// <term>FerdaBase.dll</term>
    /// <description>
    /// <para>
    /// Tato knihovna obsahuje datové struktury a datové typy, se kterımi 
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
    /// Mimo jiné obsahuje tøídu <see cref="T:Ferda.Modules.BoxModuleI"/>, která je 
    /// nezbytná pro implementaci krabièky ... implementuje toti rozhraní pro komunikaci
    /// s ModulesManagerem a ProjectManagerem (viz. dokumentace k Architekture Ferdy).
    /// Pro implementaci novıch krabièek je proto nezbytná implementace interface 
    /// <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>. Tato knihovna obsahuje øadu pomocnıch 
    /// tøíd o kterıch budeme hovoøit pozdìji.
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
    /// Obsahuje kód vygenerovanı ze slice návrhù krabièek distribuovanıch s Ferdou 
    /// tj. tato knihovna je nezbytná pokud chcete volat funkce tìchto krabièek.
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
    /// Píšete-li vlastní slubu, v ni pobìí Vámi vytvoøené krabièky (postup viz. níe) 
    /// implementujete abstraktní tøídu <see cref="T:Ferda.Modules.FerdaServiceI"/>, která 
    /// zase implementuje interface IceBox.Service - Ten je definován v iceboxcs.dll.
    /// </para>
    /// <para>
    /// Tato knihovna je distribuována mj. s Ice.
    /// </para>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <b>Implementace krabièky</b>
    /// <para>
    /// Na krabièku (nìkdy té BoxModule nebo jen Module) se mùeme dívat z dvou rùznıch
    /// pohledù.
    /// <list type="number">
    /// <item>
    /// <term>Krabièka z pohledu Project Manageru a Modules Manageru</term>
    /// <description>
    /// <para>
    /// Kadá krabièka musí implementovat øadu funkcí, které umoní Modules (nìkdy Project)
    /// Manageru zprostøedkovávat rùzné informace/data a funkcionalitu Front-Endu (nìkdy jinım 
    /// krabièkám). Pøíkladem takovıch funkcí mohou bıt funkce pro získání seznamu a popisu 
    /// vlastností (property) a zásuvek (socket) nebo napøíklad labelu krabièky èi nápovìdy 
    /// ke krabièce atd. Zájemci se mohou podívat napøíklad na následující slice návrhy 
    /// <c>Ferda.Modules.BoxModule</c>, <c>Ferda.Modules.BoxModuleFactory</c> a 
    /// <c>Ferda.Modules.BoxModuleFactoryCreator</c>; pøípadnì pøímo na dokumentaci k implementaci 
    /// tìchto návrhù (FerdaServerBase.dll) <see cref="T:Ferda.Modules.BoxModuleI"/>, 
    /// <see cref="T:Ferda.Modules.BoxModuleFactoryI"/> a 
    /// <see cref="T:Ferda.Modules.BoxModuleFactoryCreatorI"/>.
    /// </para>
    /// <para>
    /// Bude zde popsán nejsnadnìjší a nejefektivnìjší zpùsob jak tutu funkcionalitu 
    /// implementovat. Jak ji bylo uvedeno vıše pøíslušné slice návrhy jsou ji 
    /// implementovány v knihovnì FerdaServerBase.dll, kde je také øada pomocnıch funkcí.
    /// Zbıvá ovšem implemetnovat interface <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>.
    /// Ten od nás odstínil nìkteré detaily implementace vıše zmínìnıch slice návrhù.
    /// </para>
    /// <para>
    /// Interface <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/> mùete imlementovat 
    /// pøímo, co je ovšem ponìkud zdlouhavı proces, nebo je zde potøeba øešit napøíklad
    /// lokalizaci apod. Proto zde popíšeme daleko rychlejší zpùsob. Je pøipraveno nìkolik
    /// tøíd (rovnì ve FerdaServerBase.dll), pomocí nich budete mnoha práce zproštìni.
    /// Budete muset pouze implementovat abstraktní tøídu <see cref="T:Ferda.Modules.Boxes.BoxInfo"/>
    /// tj. asi 6 funkcí a bude-li to tøeba pøetííte nìkolik virtuálních funkcí:
    /// <list type="table">
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.GetPropertyObjectFromInterface(System.String,Ice.ObjectPrx)"/></term>
    /// <description>
    /// Implementujte, pokud pouíváte „nestadardní“ datovı typ pro vlastnost/zásuvku. 
    /// Implicitnì vrací null. TODO
    /// </description>
    /// </item>
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.GetReadOnlyPropertyValue(System.String,Ferda.Modules.BoxModuleI)"/></term>
    /// <description>
    /// Implementujte, pokud Vaše krabièka má nìjakou readonly vlastnost 
    /// (zde implementujete její vıpoèet). Není-li implementována pøestoe 
    /// je potøeba, je vyvolána vıjimka.
    /// </description>
    /// </item>
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.IsPropertySet(System.String,Ferda.Modules.PropertyValue)"/></term>
    /// <description>
    /// Implementujte, pokud pouíváte „nestadardní“ datovı typ (kterı nepouíváte 
    /// pouze jako readonly vlastnost). Umoní (nejen) Front-Endu rozpoznat, kdy 
    /// je vlastnost ji nastavena. Implicitnì vrací <c>true</c>.
    /// </description>
    /// </item>
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.GetDynamicHelpItems(System.String[],Ferda.Modules.BoxModuleI)"/></term>
    /// <description>
    /// Tato funkce implicitnì vrací všechny definované poloky dynamické (v tuto 
    /// chvíli ještì nedynamické) nápovìdy. Chcete-li, aby se obsah nápovedy 
    /// dynamicky mìnil podle vnitøního stavu krabièky, pøetìte tuto funkci.
    /// </description>
    /// </item>
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.RunAction(System.String,Ferda.Modules.BoxModuleI)"/></term>
    /// <description>
    /// Implementujte, pokud má Vaše krabièka nìjaké 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Actions">akce</see>
    /// (které uivatel explicitnì volá z Front-Endu). Není-li implementována 
    /// pøestoe je potøeba, je vyvolána vyvolá vıjimka.
    /// </description>
    /// </item>
    /// </list>
    /// K abstraktním i vıše uvedenım virtuálním funkcím jsou v programátorské nápovìdì 
    /// uvedeny pøíklady, jak je implementovat, proto je zde nebudu dále komentovat.
    /// Nezbytnım krokem pøi implementaci interfacu <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>
    /// s pomocí abstraktní tøídy <see cref="T:Ferda.Modules.Boxes.BoxInfo"/> je vytvoøení
    /// konfiguraèních a lokalizaèních XML souborù a jejich správné umístìní v adresáøové
    /// struktuøe Ferdy (TODO zdokumentovat).
    /// </para>
    /// <b>Konfiguraèní a lokalizaèní XML soubory</b>
    /// <para>
    /// Konfiguraèní a lokalizaèní soubory vytváøejte podle pøíslušnıch XSD schémat 
    /// a nápovìdy:
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
    /// Pøi konstrukci objektu <see cref="T:Ferda.Modules.Boxes.BoxInfo"/> se tyto
    /// XML kofiguraèní resp. lokalizaèní soubory deserealizují. Pomocí tìchto dat 
    /// je imlementována øada funkcí z <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>Krabièka z pohledu ostatních krabièek</term>
    /// <description>
    /// <para>
    /// Krabièka z poledu ostatních krabièek poskytuje urèitou funkci resp. funkce (vlastní 
    /// funkcionalita krabièky). V tuto chvíli je vhodné se podívat na dokumentaci 
    /// k formalizmu principu zapojování krabièek do zásuvek (TODO Michal). Na tomto místì uvedu jen 
    /// velmi struèné a nepøesné (avšak v tuto chvili snad postaèující) vysvìtlení:
    /// kadá krabièka implementuje urèitı interface (tj funkci/mnoinu funkcí) resp. 
    /// nìkolik intefacu. Tyto interface jsou (vnitøním mechanismem Ice) pojmenovány 
    /// a zásuvky (sokety) pak uvedou 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.BoxType.FunctionIceId">
    /// seznam jmen interfacu</see>, které akceptují.
    /// </para>
    /// <para>
    /// Uveïme na tomto místì jednoduchı pøíklad toho, jak implementovat 
    /// funkcionalitu krabièky z pohledu ostatních krabièek. Nejprve je tøeba 
    /// vytvoøit slice návrh (interface) a definovat tak funkci/mnoinu funkcí, 
    /// které krabièka implementuje. V následujícím pøíkladu definujeme dva interfacei.
    /// Naše krabièka, která pak bude implementovat intervace 
    /// <c>MyBoxModule.MyBoxModuleFunctions</c> bude moci bıt zasunuta do zásuvek 
    /// akceptujících alespoò jeden z následujícíh (pojmenovanych) interfacu: 
    /// <c>::MyBoxModule::MyBoxModuleFunctionsCore</c>
    /// nebo <c>::MyBoxModule::MyBoxModuleFunctions</c>. 
    /// </para>
    /// <code>
    /// //soubor MyBoxModule.ice
    /// module MyBoxModule
    /// {
    ///     interface MyBoxModuleFunctionsCore
    ///     {
    ///         // tato funkce vráti øetìzec "Hello World!"
    ///         nonmutating string HelloWorld();
    ///     };
    /// 
    /// 	interface MyBoxModuleFunctions extends MyBoxModule::MyBoxModuleFunctionsCore
    /// 	{
    ///         // tato funkce vrátí vstupní øetìzec (echoString)
    ///         nonmutating string Echo(string echoString);
    /// 
    /// 		/* ... další funkce ... */
    /// 	};
    /// };
    /// </code>
    /// <para>
    /// Nyní pomocí utility slie2cs vygenerujeme z našeho slice návrhu 
    /// (soubor MyBoxModule.ice) C# kód (jeho obsah nás pøíliš/vùbec 
    /// nemusí zajímat). Postup generování je dobøe popsán k dokumentaci 
    /// k Ice, zde uvedeme jen nástin a jednoduchı pøíklad:
    /// Nech cesta ke adresáøi Ice/bin (napø. c:\Ice-3.0.0\bin\") je 
    /// v cestì (systémová promìnná PATH) pak následujícím pøíkazem 
    /// (z pøíkazové øádky resp. v shellu) vygenerujeme C# kód z našich
    /// slice návrhù.
    /// <code>
    /// $ slice2cs [options] slice-files
    /// Options:
    /// - Argument -IDIR, kde DIR je cesta k *.ice souborùm tj. slice návrhùm, 
    ///   na které se v našem slice návru odkazujeme pomocí direktivy #include
	/// - Argument --output-dir je cesta do adresáøe, kam se vısledné C# (*.cs) 
    ///   soubor/y vygenerují.
    /// Slice-Files:
	/// - Seznam souborù ke zpracování (vygeneruje *.cs ze zadanıch *.ice)
    /// 
    /// Více z pøíkazové øádky pomocí $ slice2cs --help
    /// </code>
    /// V našem pøípadì tedy:
    /// <code>
    /// $ mkdir generated
    /// $ slice2cs --output-dir generated MyBoxModule.ice
    /// </code>
    /// </para>
    /// <para>
    /// Jak ji bylo uvedeno vıše naše krabièka bude implementovat inferface 
    /// <c>MyBoxModule.MyBoxModuleFunctions</c> (chcete-li 
    /// <c>::MyBoxModule::MyBoxModuleFunctions</c>), toho dosáhneme tak, e 
    /// vytvoøíme novu tøídu, která bude implementovat abstraktní (vygenerovanou 
    /// tøídu) <c>MyBoxModuleFunctionsDisp_</c> (jmenuje se stejne jako námi 
    /// definovanı interface ve slice návrhu + øetìzec "<c>Disp_</c>").
    /// Povšimnìte si prosím, e tøída implementující funkce krabièky musí 
    /// rovnì implementovat interface <see cref="T:Ferda.Modules.IFunctions"/>
    /// (implementuje se vdy stejnì). Zde jen naokraj uvedu, e z modulu 
    /// (ve slice návrhu) <c>MyBoxModule</c> byl vygenerován stejnojmennı 
    /// <c>namespace</c> (více v dokumentaci k Ice).
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
    /// <b>Registrace krabièky</b>
    /// TODO Michal ... + zdokumentovat tu Service svini :-)
    /// <para>
    /// Vytvoøte tøídu, která implementuje abstraktní tøídu 
    /// <see cref="T:Ferda.Modules.FerdaServiceI"/>. K tomu 
    /// staèí implementovat (triviálnì) jednu její metodu 
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
    /// Nyní je potøeba ... tj conf/db/application.xml, odinstalovat + nainstalovat (refresh)
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
