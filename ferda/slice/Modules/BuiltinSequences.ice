#ifndef FERDA_MODULES_BUILTIN_SEQUENCES
#define FERDA_MODULES_BUILTIN_SEQUENCES

module Ferda {
	module Modules {
		sequence<byte> ByteSeq;
		sequence<string> StringSeq;
		sequence<StringSeq> StringSeqSeq;
		sequence<int> IntSeq;
		sequence<double> DoubleSeq;
		sequence<IntSeq> IntSeqSeq;

		sequence<bool> BoolOpt;
		sequence<short> ShortOpt;
		sequence<int> IntOpt;
		sequence<long> LongOpt;
		sequence<float> FloatOpt;
		sequence<double> DoubleOpt;
		sequence<string> StringOpt;
		sequence<byte> ByteSeqOpt;
	};
};

#endif
