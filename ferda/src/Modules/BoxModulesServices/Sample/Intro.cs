using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.Sample
{
    /// <summary>
    /// <para>
    /// Tento �l�nek popisuje postup, jak�m lze relativn� snadno p�idat 
    /// vlastn� krabi�ku do syst�mu �Ferda�. Tento �l�nek popisuje postup 
    /// pr�ce ve v�vojov�m prost�ed� Microsoft Visual Studia 2005 za pou�it�
    /// jazyka C# verze 2.0. Analogicky lze pou��t i jin� prost�ed� a jazyky
    /// podporovan� platformou .NET nebo ICE.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <b>Prost�edky</b>
    /// <para>Pot�ebn� n�stroje
    /// <list type="bullet">
    /// <item>
    /// <term><see href="http://www.zeroc.com">Ice</see></term>
    /// <description>
    /// Na stroji, kde chcete pou�t�t Ferdu mus� b�t Ice nainstalov�no.
    /// D�le budeme p�i implementaci krabi�ek pot�ebovat utilitu <b>slice2cs</b>,
    /// kter� bude generovat C# k�d z na�ich slice n�vrh� krabi�ek (viz. d�le).
    /// V neposledn� �ad� budeme pot�ebovat p�i build�n� projektu reference na 
    /// n�kolik Ice knihoven (viz. d�le).
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>Pot�ebn� reference
    /// <list type="bullet">
    /// <item>
    /// <term>FerdaBase.dll</term>
    /// <description>
    /// <para>
    /// Tato knihovna obsahuje datov� struktury a datov� typy, se kter�mi 
    /// syst�m Ferda pracuje, ...
    /// </para>
    /// <para>
    /// Tato knihovna je distribuov�na spolu s Ferdou.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>FerdaServerBase.dll</term>
    /// <description>
    /// <para>
    /// Mimo jin� obsahuje t��du <see cref="T:Ferda.Modules.BoxModuleI"/>, kter� je 
    /// nezbytn� pro implementaci krabi�ky ... implementuje toti� rozhran� pro komunikaci
    /// s ModulesManagerem a ProjectManagerem (viz. dokumentace k Architekture Ferdy).
    /// Pro implementaci nov�ch krabi�ek je proto nezbytn� implementace interface 
    /// <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>. Tato knihovna obsahuje �adu pomocn�ch 
    /// t��d o kter�ch budeme hovo�it pozd�ji.
    /// </para>
    /// <para>
    /// Tato knihovna je distribuov�na spolu s Ferdou.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>FerdaBoxInterfaces.dll</term>
    /// <description>
    /// <para>
    /// Obsahuje k�d vygenerovan� ze slice n�vrh� krabi�ek distribuovan�ch s Ferdou 
    /// tj. tato knihovna je nezbytn� pokud chcete volat funkce t�chto krabi�ek.
    /// </para>
    /// <para>
    /// Tato knihovna je distribuov�na spolu s Ferdou.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>icecs.dll</term>
    /// <description>
    /// <para>
    /// Knihovna pro pr�ci s Ice v jazyce C#.
    /// </para>
    /// <para>
    /// Tato knihovna je distribuov�na mj. s Ice.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>iceboxcs.dll</term>
    /// <description>
    /// <para>
    /// P�ete-li vlastn� slu�bu, v ni� pob�� V�mi vytvo�en� krabi�ky (postup viz. n�e) 
    /// implementujete abstraktn� t��du <see cref="T:Ferda.Modules.FerdaServiceI"/>, kter� 
    /// zase implementuje interface IceBox.Service - Ten je definov�n v iceboxcs.dll.
    /// </para>
    /// <para>
    /// Tato knihovna je distribuov�na mj. s Ice.
    /// </para>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <b>Implementace krabi�ky</b>
    /// <para>
    /// Na krabi�ku (n�kdy t� BoxModule nebo jen Module) se m��eme d�vat z dvou r�zn�ch
    /// pohled�.
    /// <list type="number">
    /// <item>
    /// <term>Krabi�ka z pohledu Project Manageru a Modules Manageru</term>
    /// <description>
    /// <para>
    /// Ka�d� krabi�ka mus� implementovat �adu funkc�, kter� umo�n� Modules (n�kdy Project)
    /// Manageru zprost�edkov�vat r�zn� informace/data a funkcionalitu Front-Endu (n�kdy jin�m 
    /// krabi�k�m). P��kladem takov�ch funkc� mohou b�t funkce pro z�sk�n� seznamu a popisu 
    /// vlastnost� (property) a z�suvek (socket) nebo nap��klad labelu krabi�ky �i n�pov�dy 
    /// ke krabi�ce atd. Z�jemci se mohou pod�vat nap��klad na n�sleduj�c� slice n�vrhy 
    /// <c>Ferda.Modules.BoxModule</c>, <c>Ferda.Modules.BoxModuleFactory</c> a 
    /// <c>Ferda.Modules.BoxModuleFactoryCreator</c>; p��padn� p��mo na dokumentaci k implementaci 
    /// t�chto n�vrh� (FerdaServerBase.dll) <see cref="T:Ferda.Modules.BoxModuleI"/>, 
    /// <see cref="T:Ferda.Modules.BoxModuleFactoryI"/> a 
    /// <see cref="T:Ferda.Modules.BoxModuleFactoryCreatorI"/>.
    /// </para>
    /// <para>
    /// Bude zde pops�n nejsnadn�j�� a nejefektivn�j�� zp�sob jak tutu funkcionalitu 
    /// implementovat. Jak ji� bylo uvedeno v��e p��slu�n� slice n�vrhy jsou ji� 
    /// implementov�ny v knihovn� FerdaServerBase.dll, kde je tak� �ada pomocn�ch funkc�.
    /// Zb�v� ov�em implemetnovat interface <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>.
    /// Ten od n�s odst�nil n�kter� detaily implementace v��e zm�n�n�ch slice n�vrh�.
    /// </para>
    /// <para>
    /// Interface <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/> m��ete imlementovat 
    /// p��mo, co� je ov�em pon�kud zdlouhav� proces, nebo� je zde pot�eba �e�it nap��klad
    /// lokalizaci apod. Proto zde pop�eme daleko rychlej�� zp�sob. Je p�ipraveno n�kolik
    /// t��d (rovn� ve FerdaServerBase.dll), pomoc� nich� budete mnoha pr�ce zpro�t�ni.
    /// Budete muset pouze implementovat abstraktn� t��du <see cref="T:Ferda.Modules.Boxes.BoxInfo"/>
    /// tj. asi 6 funkc� a bude-li to t�eba p�et��te n�kolik virtu�ln�ch funkc�:
    /// <list type="table">
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.GetPropertyObjectFromInterface(System.String,Ice.ObjectPrx)"/></term>
    /// <description>
    /// Implementujte, pokud pou��v�te �nestadardn� datov� typ pro vlastnost/z�suvku. 
    /// Implicitn� vrac� null. TODO
    /// </description>
    /// </item>
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.GetReadOnlyPropertyValue(System.String,Ferda.Modules.BoxModuleI)"/></term>
    /// <description>
    /// Implementujte, pokud Va�e krabi�ka m� n�jakou readonly vlastnost 
    /// (zde implementujete jej� v�po�et). Nen�-li implementov�na p�esto�e 
    /// je pot�eba, je vyvol�na v�jimka.
    /// </description>
    /// </item>
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.IsPropertySet(System.String,Ferda.Modules.PropertyValue)"/></term>
    /// <description>
    /// Implementujte, pokud pou��v�te �nestadardn� datov� typ (kter� nepou��v�te 
    /// pouze jako readonly vlastnost). Umo�n� (nejen) Front-Endu rozpoznat, kdy 
    /// je vlastnost ji� nastavena. Implicitn� vrac� <c>true</c>.
    /// </description>
    /// </item>
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.GetDynamicHelpItems(System.String[],Ferda.Modules.BoxModuleI)"/></term>
    /// <description>
    /// Tato funkce implicitn� vrac� v�echny definovan� polo�ky dynamick� (v tuto 
    /// chv�li je�t� nedynamick�) n�pov�dy. Chcete-li, aby se obsah n�povedy 
    /// dynamicky m�nil podle vnit�n�ho stavu krabi�ky, p�et�te tuto funkci.
    /// </description>
    /// </item>
    /// <item>
    /// <term><see cref="M:Ferda.Modules.Boxes.BoxInfo.RunAction(System.String,Ferda.Modules.BoxModuleI)"/></term>
    /// <description>
    /// Implementujte, pokud m� Va�e krabi�ka n�jak� 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Actions">akce</see>
    /// (kter� u�ivatel explicitn� vol� z Front-Endu). Nen�-li implementov�na 
    /// p�esto�e je pot�eba, je vyvol�na vyvol� v�jimka.
    /// </description>
    /// </item>
    /// </list>
    /// K abstraktn�m i v��e uveden�m virtu�ln�m funkc�m jsou v program�torsk� n�pov�d� 
    /// uvedeny p��klady, jak je implementovat, proto je zde nebudu d�le komentovat.
    /// Nezbytn�m krokem p�i implementaci interfacu <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>
    /// s pomoc� abstraktn� t��dy <see cref="T:Ferda.Modules.Boxes.BoxInfo"/> je vytvo�en�
    /// konfigura�n�ch a lokaliza�n�ch XML soubor� a jejich spr�vn� um�st�n� v adres��ov�
    /// struktu�e Ferdy (TODO zdokumentovat).
    /// </para>
    /// <b>Konfigura�n� a lokaliza�n� XML soubory</b>
    /// <para>
    /// Konfigura�n� a lokaliza�n� soubory vytv��ejte podle p��slu�n�ch XSD sch�mat 
    /// a n�pov�dy:
    /// <list type="bullet">
    /// <listheader>
    /// <term>dokumentace</term>
    /// <description>XSD sch�mata</description>
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
    /// P�i konstrukci objektu <see cref="T:Ferda.Modules.Boxes.BoxInfo"/> se tyto
    /// XML kofigura�n� resp. lokaliza�n� soubory deserealizuj�. Pomoc� t�chto dat 
    /// je imlementov�na �ada funkc� z <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>Krabi�ka z pohledu ostatn�ch krabi�ek</term>
    /// <description>
    /// <para>
    /// Krabi�ka z poledu ostatn�ch krabi�ek poskytuje ur�itou funkci resp. funkce (vlastn� 
    /// funkcionalita krabi�ky). V tuto chv�li je vhodn� se pod�vat na dokumentaci 
    /// k formalizmu principu zapojov�n� krabi�ek do z�suvek (TODO Michal). Na tomto m�st� uvedu jen 
    /// velmi stru�n� a nep�esn� (av�ak v tuto chvili snad posta�uj�c�) vysv�tlen�:
    /// ka�d� krabi�ka implementuje ur�it� interface (tj funkci/mno�inu funkc�) resp. 
    /// n�kolik intefacu. Tyto interface jsou (vnit�n�m mechanismem Ice) pojmenov�ny 
    /// a z�suvky (sokety) pak uvedou 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.BoxType.FunctionIceId">
    /// seznam jmen interfacu</see>, kter� akceptuj�.
    /// </para>
    /// <para>
    /// Uve�me na tomto m�st� jednoduch� p��klad toho, jak implementovat 
    /// funkcionalitu krabi�ky z pohledu ostatn�ch krabi�ek. Nejprve je t�eba 
    /// vytvo�it slice n�vrh (interface) a definovat tak funkci/mno�inu funkc�, 
    /// kter� krabi�ka implementuje. V n�sleduj�c�m p��kladu definujeme dva interfacei.
    /// Na�e krabi�ka, kter� pak bude implementovat intervace 
    /// <c>MyBoxModule.MyBoxModuleFunctions</c> bude moci b�t zasunuta do z�suvek 
    /// akceptuj�c�ch alespo� jeden z n�sleduj�c�h (pojmenovanych) interfacu: 
    /// <c>::MyBoxModule::MyBoxModuleFunctionsCore</c>
    /// nebo <c>::MyBoxModule::MyBoxModuleFunctions</c>. 
    /// </para>
    /// <code>
    /// //soubor MyBoxModule.ice
    /// module MyBoxModule
    /// {
    ///     interface MyBoxModuleFunctionsCore
    ///     {
    ///         // tato funkce vr�ti �et�zec "Hello World!"
    ///         nonmutating string HelloWorld();
    ///     };
    /// 
    /// 	interface MyBoxModuleFunctions extends MyBoxModule::MyBoxModuleFunctionsCore
    /// 	{
    ///         // tato funkce vr�t� vstupn� �et�zec (echoString)
    ///         nonmutating string Echo(string echoString);
    /// 
    /// 		/* ... dal�� funkce ... */
    /// 	};
    /// };
    /// </code>
    /// <para>
    /// Nyn� pomoc� utility slie2cs vygenerujeme z na�eho slice n�vrhu 
    /// (soubor MyBoxModule.ice) C# k�d (jeho� obsah n�s p��li�/v�bec 
    /// nemus� zaj�mat). Postup generov�n� je dob�e pops�n k dokumentaci 
    /// k Ice, zde uvedeme jen n�stin a jednoduch� p��klad:
    /// Nech� cesta ke adres��i Ice/bin (nap�. c:\Ice-3.0.0\bin\") je 
    /// v cest� (syst�mov� prom�nn� PATH) pak n�sleduj�c�m p��kazem 
    /// (z p��kazov� ��dky resp. v shellu) vygenerujeme C# k�d z na�ich
    /// slice n�vrh�.
    /// <code>
    /// $ slice2cs [options] slice-files
    /// Options:
    /// - Argument -IDIR, kde DIR je cesta k *.ice soubor�m tj. slice n�vrh�m, 
    ///   na kter� se v na�em slice n�vru odkazujeme pomoc� direktivy #include
	/// - Argument --output-dir je cesta do adres��e, kam se v�sledn� C# (*.cs) 
    ///   soubor/y vygeneruj�.
    /// Slice-Files:
	/// - Seznam soubor� ke zpracov�n� (vygeneruje *.cs ze zadan�ch *.ice)
    /// 
    /// V�ce z p��kazov� ��dky pomoc� $ slice2cs --help
    /// </code>
    /// V na�em p��pad� tedy:
    /// <code>
    /// $ mkdir generated
    /// $ slice2cs --output-dir generated MyBoxModule.ice
    /// </code>
    /// </para>
    /// <para>
    /// Jak ji� bylo uvedeno v��e na�e krabi�ka bude implementovat inferface 
    /// <c>MyBoxModule.MyBoxModuleFunctions</c> (chcete-li 
    /// <c>::MyBoxModule::MyBoxModuleFunctions</c>), toho dos�hneme tak, �e 
    /// vytvo��me novu t��du, kter� bude implementovat abstraktn� (vygenerovanou 
    /// t��du) <c>MyBoxModuleFunctionsDisp_</c> (jmenuje se stejne jako n�mi 
    /// definovan� interface ve slice n�vrhu + �et�zec "<c>Disp_</c>").
    /// Pov�imn�te si pros�m, �e t��da implementuj�c� funkce krabi�ky mus� 
    /// rovn� implementovat interface <see cref="T:Ferda.Modules.IFunctions"/>
    /// (implementuje se v�dy stejn�). Zde jen naokraj uvedu, �e z modulu 
    /// (ve slice n�vrhu) <c>MyBoxModule</c> byl vygenerov�n stejnojmenn� 
    /// <c>namespace</c> (v�ce v dokumentaci k Ice).
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
    ///			/* ... dal�� funkce ... */
    ///		}
    /// }
    /// </code>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <b>Registrace krabi�ky</b>
    /// TODO Michal ... + zdokumentovat tu Service svini :-)
    /// <para>
    /// Vytvo�te t��du, kter� implementuje abstraktn� t��du 
    /// <see cref="T:Ferda.Modules.FerdaServiceI"/>. K tomu 
    /// sta�� implementovat (trivi�ln�) jednu jej� metodu 
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
    /// Nyn� je pot�eba ... tj conf/db/application.xml, odinstalovat + nainstalovat (refresh)
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
