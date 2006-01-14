
using System;
using Ice;

namespace Ferda.Modules
{
	public class ObjectFactoryForPropertyTypes : Ice.LocalObjectImpl, Ice.ObjectFactory
	{
		
		/// <summary>
		/// Method create
		/// </summary>
		/// <returns>An Ice.Object</returns>
		/// <param name="type">A  string</param>
		public Ice.Object create(string type)
		{
			switch(type)
			{
				case "::Ferda::Modules::BoolT":
					return new BoolTI(false);
				case "::Ferda::Modules::ShortT":
					return new ShortTI(0);
				case "::Ferda::Modules::IntT":
					return new IntTI(0);
				case "::Ferda::Modules::LongT":
					return new LongTI(0);
				case "::Ferda::Modules::FloatT":
					return new FloatTI(0);
				case "::Ferda::Modules::DoubleT":
					return new DoubleTI(0);
				case "::Ferda::Modules::StringT":
					return new StringTI("");
				case "::Ferda::Modules::StringSeqT":
					return new StringSeqTI(new string[0]);
				case "::Ferda::Modules::TimeT":
					return new TimeTI(0,0,0);
				case "::Ferda::Modules::DateT":
					return new DateTI(0,0,0);
				case "::Ferda::Modules::DateTimeT":
					return new DateTimeTI(0,0,0,0,0,0);
				case "::Ferda::Modules::CategoriesT":
					return new CategoriesTI();
				case "::Ferda::Modules::GenerationInfoT":
					return new GenerationInfoTI();
				case "::Ferda::Modules::HypothesesT":
					return new HypothesesTI();
			}
			System.Diagnostics.Debug.Assert(false);
			return null;
		}
		
		/// <summary>
		/// Method destroy
		/// </summary>
		public void destroy()
		{
			// Nothing to do
		}

		public static void addFactoryToCommunicator(Ice.Communicator communicator,
				ObjectFactoryForPropertyTypes factory)
		{
			communicator.addObjectFactory(factory, "::Ferda::Modules::BoolT");
			communicator.addObjectFactory(factory, "::Ferda::Modules::ShortT");
			communicator.addObjectFactory(factory, "::Ferda::Modules::IntT");
			communicator.addObjectFactory(factory, "::Ferda::Modules::LongT");
			communicator.addObjectFactory(factory, "::Ferda::Modules::FloatT");
			communicator.addObjectFactory(factory, "::Ferda::Modules::DoubleT");
			communicator.addObjectFactory(factory, "::Ferda::Modules::StringT");
			communicator.addObjectFactory(factory, "::Ferda::Modules::StringSeqT");
			communicator.addObjectFactory(factory, "::Ferda::Modules::TimeT");
			communicator.addObjectFactory(factory, "::Ferda::Modules::DateT");
			communicator.addObjectFactory(factory, "::Ferda::Modules::DateTimeT");

			communicator.addObjectFactory(factory, "::Ferda::Modules::CategoriesT");
			communicator.addObjectFactory(factory, "::Ferda::Modules::GenerationInfoT");
			communicator.addObjectFactory(factory, "::Ferda::Modules::HypothesesT");
		}
	}
}
