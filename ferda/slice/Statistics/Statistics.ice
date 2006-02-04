#include <Modules/Common.ice>

module Ferda {
	module Statistics {
		interface StatisticsProvider {
		
			nonmutating float getStatistics(Ferda::Modules::AbstractQuantifierSetting quantifierSetting);
			
			nonmutating string getTaskType();

			nonmutating string getStatisticsName();
			
		};		
	};		
};

			
	