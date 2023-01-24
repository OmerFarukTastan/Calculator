using System.Data.SqlTypes;

namespace Calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Clearing Methods
        private void btCE_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = string.Empty;
            FocusInputText();
        }
        private void btDel_Click(object sender, EventArgs e)
        {
            DeleteTextValue();

            FocusInputText();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }


        #endregion

       
        #region Number Methods
        private void bt0_Click(object sender, EventArgs e)
        {
            InsertTextValue("0");
            FocusInputText();

        }

        

        private void bt1_Click(object sender, EventArgs e)
        {
            InsertTextValue("1");
            FocusInputText();
        }

        private void bt2_Click(object sender, EventArgs e)
        {
            InsertTextValue("2");
            FocusInputText();
        }

        private void bt3_Click(object sender, EventArgs e)
        {
            InsertTextValue("3");
            FocusInputText();
        }

        private void bt4_Click(object sender, EventArgs e)
        {
            InsertTextValue("4");
            FocusInputText();
        }

        private void bt5_Click(object sender, EventArgs e)
        {
            InsertTextValue("5");
            FocusInputText();
        }

        private void bt6_Click(object sender, EventArgs e)
        {
            InsertTextValue("6");
            FocusInputText();
        }

        private void bt7_Click(object sender, EventArgs e)
        {
            InsertTextValue("7");
            FocusInputText();
        }

        private void bt8_Click(object sender, EventArgs e)
        {
            InsertTextValue("8");
            FocusInputText();
        }

        private void bt9_Click(object sender, EventArgs e)
        {
            InsertTextValue("9");
            FocusInputText();
        }
        private void btDot_Click(object sender, EventArgs e)
        {
            InsertTextValue(".");
            FocusInputText();
        }

        #endregion

        #region Operator Methods
        private void btDivision_Click(object sender, EventArgs e)
        {
            InsertTextValue("/");
            FocusInputText();
        }

        private void btMultiplication_Click(object sender, EventArgs e)
        {
            InsertTextValue("*");
            FocusInputText();
        }

        private void btSubtraction_Click(object sender, EventArgs e)
        {
            InsertTextValue("-");
            FocusInputText();
        }

        private void btAddition_Click(object sender, EventArgs e)
        {
            InsertTextValue("+");
            FocusInputText();
        }

        private void btEqual_Click(object sender, EventArgs e)
        {
            CalculateEquation();
            FocusInputText();
        }



        #endregion

        #region Private Helpers

        private void FocusInputText()
        {
            this.textBox1.Focus();
        }

        private void InsertTextValue(string value)
        {
            //remember selection
            var selectionstart = this.textBox1.SelectionStart; 
            //set new test
            this.textBox1.Text = this.textBox1.Text.Insert(this.textBox1.SelectionStart, value);
            //restore the selection start
            this.textBox1.SelectionStart = selectionstart + value.Length;
            //set selection lenght to 0
            this.textBox1.SelectionLength = 0;

        }

        private void DeleteTextValue()
        {
            //if we do not have value to delete , do nothing
            if (this.textBox1.Text.Length < this.textBox1.SelectionStart + 1)
                return;


            //remember selection
            var selectionstart = this.textBox1.SelectionStart;
            //Delete the character to the right of the selection
            
            this.textBox1.Text = this.textBox1.Text.Remove(this.textBox1.SelectionStart, 1);
            //restore the selection start
            this.textBox1.SelectionStart = selectionstart;
            //set selection lenght to 0
            this.textBox1.SelectionLength = 0;

        }

        private void CalculateEquation()
        {
            

            this.CalculationResultText.Text = ParseOperation();


            FocusInputText(); 
        }

        //Prses the suers equation and calculates the result
        private string ParseOperation()
        {
            try
            {
                //Get the users equation input
                var input = this.textBox1.Text;
                //Remove all spaces
                input = input.Replace(" ", "");
                //Create a new top-level operation  
                var operation = new Operation();
                var leftSide = true;

                //loop though each character of input
                //starting from the left working to the right
                for (int i = 0; i < input.Length; i++)
                {
                    //TODO:Handle order priorty 
                    // 4 + 5 * 3
                    //It should calculate 5 * 3 first , then 4 + the result(so 4 + 15)

                    //Check if the current char is a number
                    if ("123456789.".Any(c => input[i] == c))
                    {
                        if (leftSide)
                        {
                            operation.LeftSide = AddNumberPart(operation.LeftSide, input[i]);
                        }
                        else
                        {
                            operation.RightSide = AddNumberPart(operation.RightSide, input[i]);
                        }
                    }
                    //if it is an operator (+-*/) set the other type
                    else if ("+-*/".Any(c => input[i] == c))
                    {
                        if (!leftSide)
                        {
                            //Get the operator type
                            var operatorType = GetOperationType(input[i]);




                            //Check if we actually have a right side number
                            if (operation.RightSide.Length == 0)
                            {
                                //Check the operator is not a minus (negative number)
                                if (operatorType != OperationType.Minus)
                                {
                                    throw new InvalidOperationException($"Operator (+ * / or more than one -) specified without an left side number");
                                }

                                operation.RightSide += input[i];
                            }
                            else
                            {
                                // Calculate previous equation and set to the left side
                                operation.LeftSide = CalculateOperation(operation);

                                // Set new operator
                                operation.OperationType = operatorType;

                                // Clear the previous right number
                                operation.RightSide = string.Empty;
                            }


                        }
                        else
                        {
                            // Get the operator type
                            var operatorType = GetOperationType(input[i]);

                            // Check if we actually have a left side number
                            if (operation.LeftSide.Length == 0)
                            {
                                // Check the operator is not a minus (as they could be creating a negative number)
                                if (operatorType != OperationType.Minus)
                                    throw new InvalidOperationException($"Operator (+ * / or more than one -) specified without an left side number");

                                // If we got here, the operator type is a minus, and there is no left number currently, so add the minus to the number
                                operation.LeftSide += input[i];
                            }
                            else
                            {
                                // If we get here, we have a left number and now an operator, so we want to move to the right side

                                // Set the operation type
                                operation.OperationType = operatorType;

                                // Move to the right side
                                leftSide = false;
                            }


                        }
                    }
                    
                }
                return CalculateOperation(operation);
            }
            catch (Exception ex)
            {

                return $"Invalid equation.{ex.Message}";
            }
        }

        private string CalculateOperation(Operation operation)
        {
            //store the number values of the string representations
            decimal left = 0;
            decimal right = 0;
            //check if we have a valid left side number
            if (string.IsNullOrEmpty(operation.LeftSide) || !decimal.TryParse(operation.LeftSide, out left)) 
            {
                throw new InvalidOperationException($"Left side of the operation was not a number.{operation.LeftSide}");
            }
            //check if we have a valid right side number
            if (string.IsNullOrEmpty(operation.RightSide) || !decimal.TryParse(operation.RightSide, out right)) 
            {
                throw new InvalidOperationException($"Right side of the operation was not a number.{operation.RightSide}");
            }
            try
            {
                switch (operation.OperationType)
                {
                    case OperationType.Add:
                        return (left + right).ToString();
                    case OperationType.Minus:
                        return (left - right).ToString();
                    case OperationType.Divide:
                        return (left / right).ToString();
                    case OperationType.Multiply:
                        return (left * right).ToString();
                    default:
                        throw new InvalidOperationException($"Unkown operator type when calculating operation.{operation.OperationType}");

                }
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException($"Failed to calculate operation{operation.LeftSide} {operation.OperationType} {operation.RightSide} {ex.Message}");
            }

        }

        //Accepts a character and returns the known OperationType
        private OperationType GetOperationType(char character)
        {
            switch (character)
            {
                case '+':
                    return OperationType.Add;
                case '-':
                    return OperationType.Minus;
                case '/':
                    return OperationType.Divide;
                case '*':
                    return OperationType.Multiply;
                default:
                    throw new InvalidOperationException($"Unkown operator type {character}");
            }

           
        }



        //Attempts to add a new char to the current number , checking for valid characters as it goes
        private string AddNumberPart(string currentNumber, char newCharacter)
        {
            //checked if there is SqlAlreadyFilledException a . in the number
            if (newCharacter == '.' && currentNumber.Contains("."))
            {
                throw new InvalidOperationException($"Number{currentNumber} already contains a . and another cannot be added ");
            }
            return currentNumber + newCharacter;
        }


        #endregion

        private void CalculationResultText_Click(object sender, EventArgs e)
        {

        }
    }
}