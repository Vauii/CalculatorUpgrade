﻿namespace Calculator
{

    public static class ReservedIds
    {
        public const string Zero = "0";
        public const string One = "1";
    }

    public class Calculator
    {
        public static Dictionary<string, (ValueType type, double value)> values = [];
        public static List<string> expression = [];
        static List<string> operators = ["+", "-", "*", "/", "(", ")"];

        static void Main(string[] args)
        {
            PrintHelp();
            while (true)
            {
                try
                {
                    Console.Write("Enter command: ");
                    string? input = Console.ReadLine();

                    switch (input)
                    {
                        case string s when s.StartsWith("push operator"):
                            PushOperator(s);
                            PrintCurrentExpression();
                            break;
                        case string s when s.StartsWith("push value"):
                            PushValue(s);
                            PrintCurrentExpression();
                            break;
                        case string s when s.StartsWith("change value"):
                            ChangeValue(s);
                            PrintCurrentExpression();
                            break;
                        case string s when s.StartsWith("remove value"):
                            RemoveValue(s);
                            PrintCurrentExpression();
                            break;
                        case "help":
                            PrintHelp();
                            break;
                        case "compute":
                            PrintCurrentExpression();
                            Compute();
                            break;
                        default:
                            Console.WriteLine("Invalid command");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Environment.Exit(1);
                }
            }
        }

        public static void PushOperator(string input)
        {
            var parts = input.Split(' ');
            if (parts.Length != 3 || !operators.Contains(parts[2]))
            {
                throw new Exception("Invalid operator");
            }
            expression.Add(parts[2]);
        }

        public static void PushValue(string input)
        {
            var parts = input.Split(' ');
            var id = parts[3];
            if (parts.Length != 4 || !Enum.TryParse(parts[2], true, out ValueType type))
            {
                throw new Exception("Invalid value type");
            }
            if (values.ContainsKey(id))
            {
                throw new Exception("ID already exists");
            }
            if (id == ReservedIds.One || id == ReservedIds.Zero)
            {
                throw new Exception("Invalid ID");
            }
            values[id] = (type, 0);
            expression.Add(id);
        }

        public static void ChangeValue(string input)
        {
            var parts = input.Split(' ');
            var id = parts[3];
            if (parts.Length != 4)
            {
                throw new Exception("Invalid arguments");
            }
            if (!values.ContainsKey(id) || id == ReservedIds.One || id == ReservedIds.Zero)
            {
                throw new Exception("Invalid ID");
            }
            if (!double.TryParse(parts[2], out double value))
            {
                throw new Exception("Invalid value");
            }
            values[id] = (values[id].type, value);
        }

        public static void RemoveValue(string input)
        {
            var parts = input.Split(' ');
            var id = parts[2];
            if (parts.Length != 3 || !values.ContainsKey(id) || id == ReservedIds.One || id == ReservedIds.Zero)
            {
                throw new Exception("Invalid ID");
            }

            string valueStr = "";
            for (int i = 0; i < expression.Count; i++)
            {
                if (expression[i] == id)
                {
                    if (i == 0)
                    {
                        if (i + 1 < expression.Count && operators.Contains(expression[i + 1]))
                        {
                            expression[i] = expression[i + 1] switch
                            {
                                "/" => "0",
                                "*" => "1",
                                "+" => "0",
                                "-" => "0",
                                _ => "1",
                            };
                        }
                    }
                    else if (i == expression.Count - 1)
                    {
                        if (operators.Contains(expression[i - 1]))
                        {
                            expression[i] = expression[i - 1] switch
                            {
                                "/" => "0",
                                "*" => "0",
                                "+" => "1",
                                "-" => "1",
                                _ => "1",
                            };
                        }
                    }
                    else
                    {
                        string prevOp = expression[i - 1];
                        string nextOp = expression[i + 1];
                        if (operators.Contains(prevOp) && operators.Contains(nextOp))
                        {
                            if (nextOp == "/")
                            {
                                if (prevOp == "/") expression[i] = "1";
                                else expression[i] = "0";
                            }
                            else if (prevOp == "*" || nextOp == "*")
                            {
                                expression[i] = "1";
                            }
                            else
                            {
                                expression[i] = "0";
                            }
                        }
                    }
                    valueStr = expression[i];
                }
            }

            values.Remove(id);
            if (valueStr == "0") values[valueStr] = (ValueType.Scalar, 0);
            else if (valueStr == "1") values[valueStr] = (ValueType.Scalar, 1);
        }

        static void PrintHelp()
        {
            Console.WriteLine("Calculator Upgrade - Help");
            Console.WriteLine("Commands:");
            Console.WriteLine("  push operator <operator>");
            Console.WriteLine("    - <operator> from {+ - * / ( )}");
            Console.WriteLine("  push value <value_type> <id>");
            Console.WriteLine("    - <value_type> from {SpeedQuantNum, TimeQuantNum, DistanceQuantNum, Scalar}");
            Console.WriteLine($"   - <id> is not equal to '{ReservedIds.One}' or '{ReservedIds.Zero}'");
            Console.WriteLine("  change value <value> <id>");
            Console.WriteLine("    - Change the value by id");
            Console.WriteLine("  remove value <id>");
            Console.WriteLine("    - Remove the value from calculations by id");
            Console.WriteLine("  compute");
            Console.WriteLine("    - Calculate the current expressionList, output the type and value");
            Console.WriteLine("  help");
            Console.WriteLine("    - Show this help message");
            Console.WriteLine();
        }

        static void PrintCurrentExpression()
        {
            Console.WriteLine("Current expressionList with IDs: " + string.Join(" ", expression));

            List<string> expressionWithTypes = expression.Select(token =>
                values.ContainsKey(token) ? values[token].type.ToString() : token).ToList();

            List<string> expressionWithValues = expression.Select(token =>
                values.ContainsKey(token) ? values[token].value.ToString() : token).ToList();

            Console.WriteLine("Current expressionList with values: " + string.Join(" ", expressionWithValues));
            Console.WriteLine("Current expressionList with types: " + string.Join(" ", expressionWithTypes));
            Console.WriteLine();
        }


        public static void Compute()
        {
            try
            {
                var resultType = AnalyzeExpressionType(expression);
                var resultValue = AnalyzeExpressionValue(expression);
                Console.WriteLine($"Physical Quantity Type: {resultType}");
                Console.WriteLine($"Computed Value: {resultValue}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Compute Error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        public static ValueType AnalyzeExpressionType(List<string> expressionList)
        {
            var postfix = ConvertToPostfix(expressionList);
            var resultCounter = EvaluatePostfixType(postfix);
            var singleType = resultCounter.GetSingleType();
            if (singleType.HasValue) return singleType.Value;
            throw new Exception("Resulting in unknown physical quantity");
        }

        public static double AnalyzeExpressionValue(List<string> expressionList)
        {
            var postfix = ConvertToPostfix(expressionList);
            return EvaluatePostfixValue(postfix);
        }


        static List<string> ConvertToPostfix(List<string> infix)
        {
            Dictionary<string, int> precedence = new Dictionary<string, int>
            {
                { "+", 1 },
                { "-", 1 },
                { "*", 2 },
                { "/", 2 },
                { "(", 0 }
            };

            List<string> postfix = [];
            Stack<string> stack = new();

            foreach (var token in infix)
            {
                if (values.ContainsKey(token))
                {
                    postfix.Add(token);
                }
                else if (token == "(")
                {
                    stack.Push(token);
                }
                else if (token == ")")
                {
                    while (stack.Count > 0 && stack.Peek() != "(")
                    {
                        postfix.Add(stack.Pop());
                    }
                    stack.Pop();
                }
                else
                {
                    while (stack.Count > 0 && precedence[token] <= precedence[stack.Peek()])
                    {
                        postfix.Add(stack.Pop());
                    }
                    stack.Push(token);
                }
            }

            while (stack.Count > 0)
            {
                postfix.Add(stack.Pop());
            }

            return postfix;
        }

        static QuantityCounter EvaluatePostfixType(List<string> postfix)
        {
            Stack<QuantityCounter> stack = new();

            foreach (var token in postfix)
            {
                if (values.ContainsKey(token))
                {
                    stack.Push(QuantityCounter.ToQuantityCounter(values[token].type, values[token].value));
                }
                else if (token == ReservedIds.Zero)
                {
                    stack.Push(new QuantityCounter());
                }
                else if (token == "1")
                {
                    stack.Push(QuantityCounter.ToQuantityCounter(ValueType.Scalar, 1));
                }
                else
                {
                    var right = stack.Pop();
                    var left = stack.Pop();
                    var res = EvaluateOperationType(left, right, token);
                    stack.Push(res.Simplify());
                }
            }

            return stack.Pop();
        }

        static double EvaluatePostfixValue(List<string> postfix)
        {
            Stack<double> stack = new();

            foreach (var token in postfix)
            {
                if (values.ContainsKey(token))
                {
                    stack.Push(values[token].value);
                }
                else
                {
                    var right = stack.Pop();
                    var left = stack.Pop();
                    stack.Push(EvaluateOperationValue(left, right, token));
                }
            }

            return stack.Pop();
        }

        static QuantityCounter EvaluateOperationType(QuantityCounter left, QuantityCounter right, string op)
        {
            QuantityCounter? result;
            switch (op)
            {
                
                case "+":
                    result = left + right;
                    if (result != null)
                    {
                        return result;
                    }
                    throw new Exception($"Resulting in unknown physical quantity");
                case "-":
                    result = left - right;
                    if (result != null)
                    {
                        return result;
                    }
                    throw new Exception($"Resulting in unknown physical quantity");
                case "*":
                    return left * right;
                case "/":
                    return left / right;
                case "(":
                    throw new Exception($"Unclosed brackets");
                case ")":
                    throw new Exception($"Incorrect brackets");
                default:
                    throw new Exception($"Unknown operator {op}");
            }
        }

        static double EvaluateOperationValue(double left, double right, string op)
        {
            return op switch
            {
                "/" => left / right,
                "*" => left * right,
                "+" => left + right,
                "-" => left - right,
                _ => throw new Exception($"Unknown operator {op}")
            };
        }
    }
}
