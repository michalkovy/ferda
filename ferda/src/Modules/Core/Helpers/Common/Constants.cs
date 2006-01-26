namespace Ferda.Modules.Helpers.Common
{
	public static class Constants
	{
		private const string dbNullCategoryName = "_DbNull";
		public static string DbNullCategoryName
		{
			get { return dbNullCategoryName; }
		}

        private const string emptyStringCategoryName = "_EmptyString";
        public static string EmptyStringCategoryName
        {
            get { return emptyStringCategoryName; }
        } 

		private const char leftClosedInterval = '<';
		public static char LeftClosedInterval
		{
			get { return leftClosedInterval; }
		}

		private const char rightClosedInterval = '>';
		public static char RightClosedInterval
		{
			get { return rightClosedInterval; }
		}

		private const char leftOpenedInterval = '(';
		public static char LeftOpenedInterval
		{
			get { return leftOpenedInterval; }
		} 

		private const char rightOpenedInterval = ')';
		public static char RightOpenedInterval
		{
			get { return rightOpenedInterval; }
		} 

		private const char separatorInterval = ',';
		public static char SeparatorInterval
		{
			get { return separatorInterval; }
		} 

		private const char leftEnum = '[';
		public static char LeftEnum
		{
			get { return leftEnum; }
		} 

		private const char rightEnum = ']';
		public static char RightEnum
		{
			get { return rightEnum; }
		} 

		private const char separatorEnum = ',';
		public static char SeparatorEnum
		{
			get { return separatorEnum; }
		} 

		private const char negation = '\u00AC';
		public static char Negation
		{
			get { return negation; }
		}

		private const char leftFunctionBracket = '(';
		public static char LeftFunctionBracket
		{
			get { return leftFunctionBracket; }
		}

		private const char rightFunctionBracket = ')';
		public static char RightFunctionBracket
		{
			get { return rightFunctionBracket; }
		}

		private const char leftSetBracket = '{';
		public static char LeftSetBracket
		{
			get { return leftSetBracket; }
		}

		private const char rightSetBracket = '}';
		public static char RightSetBracket
		{
			get { return rightSetBracket; }
		}

		private const char rangeSeparator = '-';
		public static char RangeSeparator
		{
			get { return rangeSeparator; }
		}

		private const char logicalAnd = '&';
		public static char LogicalAnd
		{
			get { return logicalAnd; }
		}
	}
}