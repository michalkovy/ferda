
using System;
using Ice;

namespace Ferda.Modules
{
	public static class ObjectFactoryForPropertyTypes
	{
		
		/// <summary>
		/// Method create
		/// </summary>
		/// <returns>An Ice.Object</returns>
		/// <param name="type">A  string</param>
		public static Ice.Value create(string type)
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
                //case "::Ferda::Modules::CategoriesT":
                //    return new CategoriesTI();
                //case "::Ferda::Modules::GenerationInfoT":
                //    return new GenerationInfoTI();
                //case "::Ferda::Modules::HypothesesT":
                //    return new HypothesesTI();
			}
			System.Diagnostics.Debug.Assert(false);
			return null;
		}

		public static void addFactoryToCommunicator(Ice.Communicator communicator)
		{
            lock (communicator)
            {
				var valueFactoryManager = communicator.getValueFactoryManager();
				if (valueFactoryManager.find("::Ferda::Modules::BoolT")==null)
					valueFactoryManager.add(create, "::Ferda::Modules::BoolT");
                if (valueFactoryManager.find("::Ferda::Modules::ShortT") == null)
					valueFactoryManager.add(create, "::Ferda::Modules::ShortT");
                if (valueFactoryManager.find("::Ferda::Modules::IntT") == null)
					valueFactoryManager.add(create, "::Ferda::Modules::IntT");
                if (valueFactoryManager.find("::Ferda::Modules::LongT") == null)
					valueFactoryManager.add(create, "::Ferda::Modules::LongT");
                if (valueFactoryManager.find("::Ferda::Modules::FloatT") == null)
					valueFactoryManager.add(create, "::Ferda::Modules::FloatT");
                if (valueFactoryManager.find("::Ferda::Modules::DoubleT") == null)
					valueFactoryManager.add(create, "::Ferda::Modules::DoubleT");
                if (valueFactoryManager.find("::Ferda::Modules::StringT") == null)
					valueFactoryManager.add(create, "::Ferda::Modules::StringT");
                if (valueFactoryManager.find("::Ferda::Modules::StringSeqT") == null)
					valueFactoryManager.add(create, "::Ferda::Modules::StringSeqT");
                if (valueFactoryManager.find("::Ferda::Modules::TimeT") == null)
					valueFactoryManager.add(create, "::Ferda::Modules::TimeT");
                if (valueFactoryManager.find("::Ferda::Modules::DateT") == null)
					valueFactoryManager.add(create, "::Ferda::Modules::DateT");
                if (valueFactoryManager.find("::Ferda::Modules::DateTimeT") == null)
					valueFactoryManager.add(create, "::Ferda::Modules::DateTimeT");
                
                //if (communicator.findObjectFactory("::Ferda::Modules::CategoriesT") == null)
                //    communicator.addObjectFactory(factory, "::Ferda::Modules::CategoriesT");
                //if (communicator.findObjectFactory("::Ferda::Modules::GenerationInfoT") == null)
                //    communicator.addObjectFactory(factory, "::Ferda::Modules::GenerationInfoT");
                //if (communicator.findObjectFactory("::Ferda::Modules::HypothesesT") == null)
                //    communicator.addObjectFactory(factory, "::Ferda::Modules::HypothesesT");
            }
		}
	}
}
