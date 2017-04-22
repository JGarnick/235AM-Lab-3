using Android.App;
using Android.Widget;
using Android.OS;

using System.Xml.Serialization;
using System.IO;

namespace Lab3
{
    [Activity(Label = "Lab3", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        QuoteBank quoteCollection;
        TextView quotationTextView;
        TextView quotationPersonTextView;
        string newQuoteText = "";
        string newQuotePerson = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            // Create the quote collection and display the current quote
            if(savedInstanceState == null)
            {
                quoteCollection = new QuoteBank();
                quoteCollection.LoadQuotes();
                quoteCollection.GetNextQuote();
            }
            else
            {
                string xmlQuotes = savedInstanceState.GetString("Quotes");
                XmlSerializer x = new XmlSerializer(typeof(QuoteBank));
                quoteCollection = (QuoteBank)x.Deserialize(new StringReader(xmlQuotes));
            }

            

            quotationTextView = FindViewById<TextView>(Resource.Id.quoteTextView);
            quotationPersonTextView = FindViewById<TextView>(Resource.Id.quotePersonTextView);

            //This should be the proper result based on logic above
            quotationTextView.Text = quoteCollection.CurrentQuote.Quotation;
            quotationPersonTextView.Text = "-- " + quoteCollection.CurrentQuote.Person;


            //if (savedInstanceState != null)
            //{

            //    quotationTextView.Text = savedInstanceState.GetString("current_quote_text");
            //    quotationTextView.Text = savedInstanceState.GetString("current_quote_person");
            //}
            //else
            //{
            //    quotationTextView.Text = quoteCollection.CurrentQuote.Quotation;
            //    quotationPersonTextView.Text = "-- " + quoteCollection.CurrentQuote.Person;
            //}
            
            // Display another quote when the "Next Quote" button is tapped
            var nextButton = FindViewById<Button>(Resource.Id.nextButton);
            nextButton.Click += delegate {
                quoteCollection.GetNextQuote();
                quotationTextView.Text = quoteCollection.CurrentQuote.Quotation;
                quotationPersonTextView.Text = "-- " + quoteCollection.CurrentQuote.Person;
            };

            

            var enterButton = FindViewById<Button>(Resource.Id.enterButton);

            enterButton.Click += delegate {
                //Store values from entered inputs
                newQuoteText = FindViewById<EditText>(Resource.Id.newQuoteText).Text;
                newQuotePerson = FindViewById<EditText>(Resource.Id.newQuoteByText).Text;

                //Create and add a new quote
                Quote newQuote = new Lab3.Quote { Quotation = newQuoteText, Person = newQuotePerson };

                quoteCollection.Quotes.Add(newQuote);
                quoteCollection.CurrentQuote = newQuote;
                quotationTextView.Text = quoteCollection.CurrentQuote.Quotation;
                quotationPersonTextView.Text = "-- " + quoteCollection.CurrentQuote.Person;

                FindViewById<EditText>(Resource.Id.newQuoteText).Text = "";
                FindViewById<EditText>(Resource.Id.newQuoteByText).Text = "";
            };
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            StringWriter writer = new StringWriter();

            XmlSerializer quoteSerializer = new XmlSerializer(typeof(QuoteBank));

            quoteSerializer.Serialize(writer, quoteCollection);

            string xmlQuotes = writer.ToString();
            outState.PutString("Quotes", xmlQuotes);


            //outState.PutString("current_quote_text", quoteCollection.CurrentQuote.Quotation);
            //outState.PutString("current_quote_person", quoteCollection.CurrentQuote.Person);
        }
    }
}

