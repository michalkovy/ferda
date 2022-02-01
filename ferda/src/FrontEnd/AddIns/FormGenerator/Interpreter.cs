//Interpreter and evaluater for WizardLanguage.
//Access to WizardLanguage properties.
//
// Author: Daniel Kupka<kupkd9am@post.cz>
//
// Copyright (c) 2007 Daniel Kupka
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    /// <summary>
    /// Class guarantee access to WizardLanguage properties.
    /// </summary>
     class VariableServices
     {
         /// <summary>
         /// Assotiative array with variables and their values.
         /// </summary>
        private System.Collections.Hashtable variable;

        /// <summary>
        /// Array of names of predefined variables
        /// </summary>
        private string[] predefinedVariables;

         /// <summary>
         /// Class constructor.
         /// Filling values to predefined editable and non-editable variables.
         /// </summary>
        public VariableServices()
        {
            this.predefinedVariables = new string[] {"$NUMBER_ROWS", "$RUN", 
            "$NUM_HYPOTHESES", "$HISTORY", "$FORM_INDEX", "$LIT_OFFTEN", "$BOTTOM_LIMIT", "$TOP_LIMIT", 
            "$LITERAL_LIMIT"};

            variable = new System.Collections.Hashtable();

            foreach (string __variable in predefinedVariables)
                switch (__variable)
                {
                    case "$NUMBER_ROWS": this.variable[__variable] = get_numberRows_Value();
                                         break;
                    case "$RUN": this.variable[__variable] = get_run_Value();
                                         break;
                    case "$NUM_HYPOTHESES": this.variable[__variable] = get_numberHypothesis_Value();
                                         break;
                    case "$HISTORY": this.variable[__variable] = get_history_Value();
                                         break;
                    case "$LIT_OFFTEN": this.variable[__variable] = get_literalsOfften_Value();
                                         break;
                    case "$FORM_INDEX": this.variable[__variable] = get_formIndex_Value();
                                         break;
                    case "$BOTTOM_LIMIT": this.variable[__variable] = get_bottomLimit_Value();
                                         break;
                    case "$TOP_LIMIT": this.variable[__variable] = get_topLimit_Value();
                                         break;
                    case "$LITERAL_LIMIT": this.variable[__variable] = get_literalLimit_Value();
                                         break;

                }
        }

        /// <summary>
        /// Method returns the value (Object type) of input variable.
        /// </summary>
        /// <param name="variableName">Name of variable</param>
        /// <returns>Value of input variable</returns>
        public Object getValue(string variableName)
        {
            try
            {
                return this.variable[variableName];
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Method set the value to input float variable.
        /// But not to non-editable predefined variables.
        /// </summary>
        /// <param name="variableName">Name of variable</param>
        /// <param name="variableValue">Value of variable</param>
        public void setValue(string variableName, float variableValue)
        {
            if ((variableName.IndexOf("$RUN", 0) == 0) ||
                 (variableName.IndexOf("$NUM_HYPOTHESES", 0) == 0) ||
                 (variableName.IndexOf("$NUMBER_ROWS", 0) == 0) ||
                 (variableName.IndexOf("$HISTORY", 0) == 0) ||
                 (variableName.IndexOf("$FORM_INDEX", 0) == 0) || 
                 (variableName.IndexOf("$LIT_OFFTEN", 0) == 0)) return;

            this.variable[variableName] = variableValue;
            return;
        }

        /// <summary>
        /// Method set the value of input float variable.
        /// Setting also for non-editable variables.
        /// </summary>
        /// <param name="variableName">Name of variable.</param>
        /// <param name="variableValue">Value of variable.</param>
        public void setValueP(string variableName, float variableValue)
        {
            this.variable[variableName] = variableValue;
            return;
        }

        /// <summary>
        /// Method set the value of input string variable.
        /// Setting also for non-editable variables.
        /// </summary>
        /// <param name="variableName">Name of variable.</param>
        /// <param name="variableValue">String value of variable.</param>
        public void setValueP(string variableName, string variableValue)
        {
            this.variable[variableName] = variableValue;
            return;
        }

        /// <summary>
        /// Method set the value of input string variable.
        /// </summary>
        /// <param name="variableName">Name of variable</param>
        /// <param name="variableValue">String value of variable</param>
        public void setValue(string variableName, string variableValue)
        {
            this.variable[variableName] = variableValue;
            return;
        }

        /// <summary>
        /// $NUMBER_ROWS value initialization
        /// </summary>
        private float get_numberRows_Value()
        {
             return 0;
        }

        /// <summary>
        /// $RUN value initialization
        /// </summary>
        /// <returns>Default value</returns>
        private float get_run_Value()
        {
            return 0;
        }

        private float get_numberHypothesis_Value()
        {
            return 0;
        }

        private float get_history_Value()
        {
            return 0;
        }

        public float get_literalsOfften_Value()
        {
            return 0;
        }

         public float get_formIndex_Value()
         {
             return 0;
         }

        /// <summary>
        /// editable $BOTTOM_LIMIT value initialization
        /// </summary>
        /// <returns>Default value</returns>
         public float get_bottomLimit_Value()
         {
             return 1;
         }

         /// <summary>
         /// editable $TOP_LIMIT value initialization
         /// </summary>
         /// <returns>Default value</returns>
         public float get_topLimit_Value()
         {
             return 30;
         }

         /// <summary>
         /// editable $LITERAL_LIMIT value initialization
         /// </summary>
         /// <returns>Default value</returns>
         public float get_literalLimit_Value()
         {
             return (float)0.8;
         }

    }

    /// <summary>
    /// Class divide PATH to elements.
    /// </summary>
    class PathInterpreter
    {
        /// <summary>
        /// Interpreted path.
        /// </summary>
        string path;

        /// <summary>
        /// Method split path into string array of path elements.
        /// </summary>
        /// <returns>Splitted path</returns>
        public string[] splitPath()
        {
            this.path = this.path.Trim();

            string[] path_array = this.path.Split(new string[] { "->" },  
                                                   StringSplitOptions.None );

            return path_array;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public PathInterpreter(string path)
        {
            this.path = path;
        }

    }
    /// <summary>
    /// Class interpret and evaluate set of expressions in WizardLanguage code.
    /// </summary>
    class WizardLanguageInterpreter
    {
        /// <summary>
        /// Variable service class
        /// </summary>
        VariableServices variableServices;

        /// <summary>
        /// Input string with expressions, divided by ';'.
        /// </summary>
        private String expressions;

        /// <summary>
        /// Output Error message.
        /// </summary>
       private String ERROR_message;

       /// <summary>
       /// Value and its type - float, string, bool
       /// </summary>
        private struct VALUE
        {
            public String type;
            public float value;
        }

        /// <summary>
        /// Method detect numeric constants and variables 
        /// assign values to variables
        /// </summary>
        /// <param name="expr">Input/output expression.</param>
        /// <param name="ancestor">Ancestor of actualt char.</param>
        /// <returns>Value and type of variable or constant</returns>
        private VALUE Factor(StringBuilder expr, String ancestor)
        {
            float number;
            bool sign;
            bool compare;
            bool negation = false;
            String local_variable = "";
            String operators = "+-*/()[];<>!=&|~";
            VALUE result;


            while (expr[0] == '!')
            {
                expr = expr.Remove(0, 1);
                negation=!negation;
            }
            if (expr[0] == '(')
            {
                expr = expr.Remove(0, 1);
                  if (ancestor.Equals("Multiplying")) 
                  {
                      number = Adding(expr);
                      result.type = "float";
                  }
                  else
                  {
                    compare = OR(expr);
                    
                    if (compare) number = 1;
                    else number = 0;
                    result.type = "bool";
                  }
                  expr = expr.Remove(0, 1);

                  if ((negation) && (number == 1)) number = 0;
                  else if ((negation) && (number == 0)) number = 1;

                  result.value = number;
                  return result;
            }
            else
            {
                sign = false;
                if (expr[0] == '+') expr = expr.Remove(0, 1);
                else if (expr[0] == '-')
                {
                    expr = expr.Remove(0, 1);
                    sign = true;
                }
                number = 0;
                if (expr[0] == '$')
                {
                    while (!operators.Contains(expr[0].ToString()))
                    {
                        local_variable += expr[0].ToString();
                        expr = expr.Remove(0, 1);
                    }
                    if (expr[0] == '[')
                    {
                        expr = expr.Remove(0, 1);
                        local_variable += "[";
                        number = Adding(expr);
                        local_variable += number;
                        local_variable += "]";
                        expr = expr.Remove(0, 1);
                    }
                    //if (variable.ContainsKey(local_variable))
                    //{
                      //  number = (float)variable[local_variable];
                        number = (float)this.variableServices.getValue(local_variable);
                    //}

                    result.type = "float";
                    result.value = number;

                    return result;
                }

                string string_number = "";
                while (((expr[0] >= '0') && (expr[0] <= '9')) || (expr[0] == ','))
                {
                 /*   number = number * 10 + expr[0] - '0';
                    expr = expr.Remove(0, 1);*/
                    string_number = string_number + expr[0].ToString();
                    expr = expr.Remove(0, 1);
                }
                number = float.Parse(string_number);
                if (sign) number = -number;

                result.type = "float";
                result.value = number;

                return result;
            }
         }

         /// <summary>
         /// Method evaluate mathematical expression with operator 
         /// of higher priority
         /// </summary>
         /// <param name="expr">Input/output expression.</param>
         /// <returns>Value expression after applying '*' or '/'</returns>
        private float Multiplying(StringBuilder expr)
        {
          float result;

          result = Factor(expr, "Multiplying").value;
              while ((expr[0] == '*') || (expr[0] == '/'))
              {
                  if (expr[0] == '*')
                  {
                      expr = expr.Remove(0, 1);
                      result *= Factor(expr, "Multiplying").value;
                  }
                  else if (expr[0] == '/')
                  {
                      expr = expr.Remove(0, 1);
                      result /= Factor(expr, "Multiplying").value;
                  }
              }

          return result;
        }

        /// <summary>
        /// Method evaluate mathematical expression with operator 
        /// of lower priority
        /// </summary>
        /// <param name="expr">Input/output expression.</param>
        /// <returns>Value expression after applying '+' or '-'</returns>
        private float Adding(StringBuilder expr)
        {
         float result=0;

         result = Multiplying(expr);

          while ( (expr[0] == '+') || (expr[0] == '-') )
          {
              if (expr[0] == '+')
              {
                  expr = expr.Remove(0, 1);
                  result += Multiplying(expr);
              }
              else if (expr[0] == '-')
              {
                  expr = expr.Remove(0, 1);
                  result -= Multiplying(expr);
              }
          }
          return result;
        }

        /// <summary>
        /// Method make comparison of two numbers 
        /// (expanded variables or evaluated expressions)
        /// </summary>
        /// <param name="expr">Input/output expression</param>
        /// <returns>Boolean value according to result of comparison</returns>
        private bool Comparing(StringBuilder expr)
        {
          bool result = true;
          bool akt_comparising;
          float operand1=0;
          float operand2=0;
          VALUE returned_struct;

            returned_struct = Factor(expr, "Comparing");
            if (returned_struct.type.Equals("bool"))
            {
                if (returned_struct.value == 1) result = true;
                else result = false;
                return result;
            }
            else operand1 = returned_struct.value;

            while ((expr[0] == '<') || (expr[0] == '>') || (expr[0] == '!') || (expr[0] == '='))
            {
               if ( (expr[0] == '<') && (expr[1] == '=') )
                {
                    expr = expr.Remove(0, 2);
                    operand2 = Factor(expr, "Comparing").value;
                    if (operand1 <= operand2)  akt_comparising = true;
                    else akt_comparising = false;
                    result &= akt_comparising;
                    operand2 = operand1;
                }
                else if (expr[0] == '<')
                {
                    expr = expr.Remove(0, 1);
                    operand2 = Factor(expr, "Comparing").value;
                    if (operand1 < operand2) akt_comparising = true;
                    else akt_comparising = false;
                    result &= akt_comparising;
                    operand2 = operand1;
                }
                else if ( (expr[0] == '>') && (expr[1] == '=') )
                {
                    expr = expr.Remove(0, 2);
                    operand2 = Factor(expr, "Comparing").value;
                    if (operand1 >= operand2) akt_comparising = true;
                    else akt_comparising = false;
                    result &= akt_comparising;
                    operand2 = operand1;
                }
                else if (expr[0] == '>')
                {
                    expr = expr.Remove(0, 1);
                    operand2 = Factor(expr, "Comparing").value;
                    if (operand1 > operand2) akt_comparising = true;
                    else akt_comparising = false;
                    result &= akt_comparising;
                    operand2 = operand1;
                }
                else if ( (expr[0] == '=') && (expr[1] == '=') )
                {
                    expr = expr.Remove(0, 2);
                    operand2 = Factor(expr, "Comparing").value;
                    if (operand1 == operand2) akt_comparising = true;
                    else akt_comparising = false;
                    result &= akt_comparising;
                    operand2 = operand1;
                }
                else if ( (expr[0] == '!') && (expr[1] == '=') )
                {
                    expr = expr.Remove(0, 2);
                    operand2 = Factor(expr, "Comparing").value;
                    if (operand1 != operand2) akt_comparising = true;
                    else akt_comparising = false;
                    result &= akt_comparising;
                    operand2 = operand1;
                }
            }   
            return result;
        }

        /// <summary>
        /// Method control logical AND in conditions.
        /// AND have  higher priority then OR.
        /// </summary>
        /// <param name="expr">Input/output expression.</param>
        /// <returns>Boolean value according to result application '&' </returns>
        private bool AND(StringBuilder expr)
        {
            bool result = true;

            result = Comparing(expr);

            while (expr[0] == '&')
            {
                expr = expr.Remove(0, 1);
                result &= Comparing(expr);
            }
            return result;
        }

        /// <summary>
        /// Method control logical OR in conditions.
        /// </summary>
        /// <param name="expr">Input/output expression.</param>
        /// <returns>Boolean value according to result application '|' </returns>
        private bool OR(StringBuilder expr)
        {
            bool result = true;

            result = AND(expr);

            while (expr[0] == '|')
            {
                expr = expr.Remove(0, 1);
                result |= AND(expr);
            }
            return result;
        }

        /// <summary>
        /// Method deletes some white chars.
        /// </summary>
        /// <param name="expr">Input expression.</param>
        /// <returns>Modified string</returns>
        private string Erase_white(String expr)
        {
            expr = expr.Replace(" ", "");
            expr = expr.Replace("\n", "");

            return expr;
        }

        /// <summary>
        /// Method call evaluating function according to 
        /// types of right side of expression.
        /// </summary>
        /// <param name="left">Left part of assignment</param>
        /// <param name="expr">Body of expression</param>
        private void Expression(String left, StringBuilder expr)
        {
            bool result;
            String expr_copy = expr.ToString();
            String[] ifs = new String[] { "if", "elseif", "else" };
            int counter=0;
            int length;

            //special situation
            if (expr_copy.Contains("$HISTORY["))
            {
              expr = expr.Replace(";", "");
                try
                {
                    string variable = expr.ToString(); 
                    string history = (string)variableServices.getValue(variable);
                    variableServices.setValue(left, history);
                    return;
                }
                catch (NullReferenceException)
                { }
            }

            while (counter <= 2)
            {
              length = ifs[counter].Length; 
                if (expr_copy.Substring(0, Math.Min(length, expr.Length)) == ifs[counter])
                {
                    if (counter <= 1)
                    {
                        expr = expr.Remove(0, length);
                        result = OR(expr);
                        expr_copy = expr.ToString();
                    }
                    else
                    {
                        expr = expr.Remove(0, length);
                        result = true;
                    }

                    if (result)
                    {
                        if (expr[0] == '{')
                        {
                          expr = expr.Remove(0, 1); 
                            if (expr[0] == '"')
                            {
                                expr = expr.Remove(0, 1);
                                int len = expr.Length;
                                string text = "";
                                int i = 0;
                                while ((i < len) && (expr[i] != '"'))
                                {
                                    text = text + expr[i].ToString();
                                    i++;
                                }
                                //expr = expr.Replace("\"", "");
                                //expr = expr.Replace(";", "");
                                //expr = expr.Replace("}", "");

                                //string text = expr.ToString();
                                variableServices.setValue(left, text);
                                return;
                            }
                            else
                            {
                                variableServices.setValue(left, Adding(expr));
                                return;
                            }
                        }
                    }
                   expr = expr.Remove(0, 1);
                   while (expr[0] != '}') expr = expr.Remove(0, 1);
                   expr = expr.Remove(0, 1);
                   expr_copy = expr.ToString();
                }
               else counter++; 
            }
            
            if (expr[0] == '"')
            {
                expr = expr.Replace("\"", "");
                expr = expr.Replace(";", "");

                string text = expr.ToString();

                variableServices.setValue(left, text);
                //variable[left] = expr;
            }
            else //variable[left] = (float)Adding(expr);
                variableServices.setValue(left, Adding(expr));
          
        }

        /// <summary>
        /// Method divide variable expressions by ';' char, for each part
        /// is called expression() function.
        /// </summary>
        private void ParseExpressions()
        {
            String expression;
            StringBuilder formated_expression;
            String left_part = "";
            String right_part = "";
            int index_equals;
            int start_index;

            while ((start_index = expressions.IndexOf(';')) != -1)
            {
                expression = expressions.Substring(0, start_index);
                expressions = expressions.Substring(start_index + 1);

                expression = Erase_white(expression);
               // expression = expression.Trim();

                index_equals = expression.IndexOf('=');
                left_part = expression.Substring(0, index_equals);
                right_part =
                     expression.Substring(index_equals + 1, expression.Length - index_equals - 1);
                right_part += ";";

                formated_expression = new StringBuilder(right_part);

                try
                {
                    Expression(left_part, formated_expression);
                }
                catch (ArithmeticException)
                {
                    ERROR_message = "Mathematic error !!!";
                    throw new Exception(ERROR_message);
                }
            }
            //variableServices.get_literalsOfften_Value();
        }

        /// <summary>
        /// Constructor of WizardLanguageInterpreter class
        /// </summary>
        /// <param name="expr">Parsed expressions</param>
        /// <param name="variableServices">Class with variables</param>
        public WizardLanguageInterpreter(String expr, VariableServices variableServices)
        {
            this.expressions = expr;

            this.variableServices = variableServices;

            ParseExpressions();
        }

    }

  /*  class DebugClass
    {
        static void Main(string[] args)
        {
            string e = "$c =  $a[1] +    \n  2 /   4;$d=1;$e = $c + 2 * 3 + 4;   $f = \"debug_text\"; $g = if ((6 < $c) | (10 < $e)) {$c+3}  elseif (4 != $c){$e*2} else {$c+20};";

             VariableServices s = new VariableServices();

             WizardLanguageInterpreter new_interpreter = new WizardLanguageInterpreter(e, s);

            Console.ReadKey();
        }

    }*/
}
