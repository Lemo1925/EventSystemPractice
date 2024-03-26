using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using EventSystem;

public class Calculator : MonoBehaviour
{
    Button[] buttons = new Button[16];
    Text display; object ans = null;
    private class CalEventCell : IEventCell
    {
        public Button button;
        public string Name { get; set; }

        public CalEventCell(string name, Button button)
        {
            Name = name;
            this.button = button;
        }

        public string GetText() => button.GetComponentInChildren<Text>().text;
    }

    private void Awake()
    {
        display = transform.Find("displayPanel").GetComponentInChildren<Text>();
        buttons = transform.GetComponentsInChildren<Button>();
    }

    void Start()
    {
        foreach (var button in buttons)
        {
            CalEventCell gameEvent = new CalEventCell(button.name, button);
            EventManager.AddListener(button.name, MyButtonClick);
            button.onClick.AddListener(() => EventManager.Tick(gameEvent));
        }
    }
   
    public void MyButtonClick(IEventCell BtnCell)
    {
        string inputContent = ((CalEventCell)BtnCell).GetText();
        switch (inputContent)
        {
            case "Reset":
                display.text = "";
                ans = null;
                break;
            case "=":
                ans = IsFormular(display.text) ?
                    Eval(display.text) : "Illegal formula"; // 分析计算表达式得出结果
                display.text = ans.ToString();
                break;
            default:
                if (ans == null)
                    display.text += inputContent;
                else
                {
                    display.text = IsNumber(ans.ToString()) ? 
                        (IsNumber(inputContent) ? inputContent : display.text + inputContent) : "";
                    ans = null;
                }
                break;
        }
    }


    #region 对表达式分析
    public bool IsNumber(string txt) => new Regex(@"^[-]?\d").IsMatch(txt);
    public bool IsFormular(string txt) => new Regex(@"^([-]?\d+\W)*?\d+$").IsMatch(txt);

    string Precede(string p, string q)
    {
        switch (p)
        {
            case "+":
            case "-":
                return ("*/(".IndexOf(q) != -1) ? "<" : ">";
            case "*":
            case "/":
                return (q == "(") ? "<" : ">";
            case "(":
                return (q == ")") ? "=" : "<";
            case ")":
                return (q == "(") ? "?" : ">";
            case "#":
                return (q == "#") ? "=" : "<";
        }
        return "?";
    }

    Double Operate(Double a, char o, Double b)
    {
        switch (o)
        {
            case '+':
                return a + b;
            case '-':
                return a - b;
            case '*':
                return a * b;
            case '/':
                return a / b;
        }
        return 0;
    }

    public object Eval(string Expression)
    {
        Stack nArr = new Stack(), oArr = new Stack();
        int j = 0;
        char o;
        MatchCollection arr = Regex.Matches(Expression.Replace(" ", "") + "#", @"(((?<=(^|\())-)?\d+(\.\d+)?|\D)");
        oArr.Push('#');
        string w = Convert.ToString(arr[j++]);
        while (!(w == "#" && Convert.ToString(oArr.Peek()) == "#"))
        {
            if ("+-*/()#".IndexOf(w) != -1)
            {
                switch (Precede(oArr.Peek().ToString(), w))
                {
                    case "<":
                        oArr.Push(w);
                        w = Convert.ToString(arr[j++]);
                        break;
                    case "=":
                        oArr.Pop();
                        w = Convert.ToString(arr[j++]);
                        break;
                    case ">":
                        o = Convert.ToChar(oArr.Pop());
                        double b = Convert.ToDouble(nArr.Pop());
                        double a = Convert.ToDouble(nArr.Pop());
                        nArr.Push(Operate(a, o, b));
                        break;
                    default:
                        return "Error";
                }
            }
            else
            {
                nArr.Push(w);
                w = Convert.ToString(arr[j++]);
            }
        }
        return nArr.Pop();
    }
    #endregion
}
