using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Swinton.QuotesEngine.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var variableValues = new Dictionary<string, string>
                {
                    { "VariableA", "Case5" },
                    { "VariableB", "CaseX" }
                };

            ILanguageTranslator translator = new LanguageTranslator(new TextInput(new List<string> {
                "select column1",
                "from",
                "#if(VariableA = Case1)",
                "    #if(VariableB = Case3)",
                "        table3",
                "    #else",
                "        table4",
                "    #end",
                "#else if(VariableA = Case2)",
                "    table2",
                "#else",
                "    defaultTable",
                "#end"
            }));

            var parser = new StatementParser(translator, variableValues);

            MessageBox.Show(parser.Parse());
        }
    }
}
