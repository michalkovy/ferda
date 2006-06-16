namespace Ferda.Guha.Math.Quantifiers
{
    public abstract class AbstractQuantifierFunctions
    {
        public bool Validate(ContingencyTable contingecyTable, double[] numericValues, out double value)
        {
            /*
             * vraci bool ... tj. Validity
             * pokud validity je true, pak vrati i v out promenne value
             * */
            value = 0.0d;

            //pokud je value == Double.NaN pak return false;
            return true;
        }

        public QuantifierSetting GetSetting()
        {
            return null;
        }

        /* Units
         * -----
         * Budou zpracovany v Mineru a soucasti ContingecyTable je denominator
         * coz usetri nekolikere prevadeni na stejne units a usetri i operace deleni
         * protoze, ty mohou byt odlozeny do kvantifikatoru.
         * 
         * Relative To All Objects
         * Relative To Max Frequency
         * Absolute Number
         * */

        /* Treshold + otherParams[]
         * ------------------------
         * a dalsi parametry si nechava kvantifikator pro sebe
         * */

        /* Relation
         * --------
         * sam kvantifikator si ponechva pro porovnani s Treshold,
         * ale publikuje v settingu jako QuantifierSemantic
         * */

        /* FromRow, ToRow, FromColumn, ToColumn
         * ------------------------------------
         * Jsou publikovany do mineru, a ten jen posila prislusnou 
         * podtabulku kontingencni tabulky a patricnou podcast "numericValues"
         * */

        /* OperationMode
         * -------------
         * Je publikvoan Mineru a stara se o nej jen miner. Kvanitifkator ho
         * dale nezpracovava.
         * */

        /* PerformanceDifficulty
         * ---------------------
         * Poslano Mineru, kvantifikator to neresi.
         * */

        /* UseNumericValues
         * ----------------
         * (bool) dle teto informace Miner rozhoduje, zda kvantifikatoru bude 
         * posilat tato data.
         * */
    }
}