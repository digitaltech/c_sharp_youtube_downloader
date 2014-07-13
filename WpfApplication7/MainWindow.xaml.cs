using System;
using System.Collections.Generic;
using System.Collections.Specialized;//for name value collection
using System.Web; //at present for httputility
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web;//from this namespace we will be using the HttpUtility class which is a static a static class i think at present.
using System.Net; //from this at present we will be using a class called WebClient .
namespace WpfApplication7
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
        HttpUtility HttpUtility = new HttpUtility();
        WebClient Client = new WebClient();
        int NoOfLinks = 0;
        private void Download_Button_Click(object sender, RoutedEventArgs e)
        {
            try     //here an exception might arise when there is not internet connection is available
            {
                var VideoId = HttpUtility.ParseQueryString(DownloadTextBox.Text);//it has got only one query element i.e one array element so the video id will be in the zero index of the videoid variable
                var VideoInfo = "https://www.youtube.com/get_video_info?asv=3&el=detailpage&hl=en_US&video_id=" + VideoId[0];//this is a google link that will give us the complete source code of the video with all the info like link all quality videos link etc
               
                Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                Client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_StringDownloaded);
                Client.DownloadStringAsync(new Uri(VideoInfo));
            }
            catch(Exception ex1)
            {
                LblForError.Content = "There was an error during the download"+ex1;
            }
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)//this event is not firing on the right time check once
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
           /* double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100; 
            MessageBox.Show(e.TotalBytesToReceive.ToString()); */
            
           // progressBar1.Value =percentage;
        }
         void client_StringDownloaded(object sender1, DownloadStringCompletedEventArgs e1)//event handler that will be fired once the video's complete info string stream is downloaded
        {

            try
            {
             Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
             string CompleteVideoInfo=e1.Result;
             NameValueCollection CompleteEncodedUrl = HttpUtility.ParseQueryString(CompleteVideoInfo);//by default HttpUtility.ParseQueryString is of namevaluecollection type we can convert it to string
             string AllQualVideoLinks = CompleteEncodedUrl["url_encoded_fmt_stream_map"]; //from all namevalue collction remove out name "url_encoded_fmt_stream_map" which contains url or link to all different quality of the video
             var SplitQualLinks = AllQualVideoLinks.Split(new Char[] { ',' }); //different links to dfferent quality videos is separated by ',' here
             int SplitQualLinksLen = SplitQualLinks.Length;//get the total length of the array
             string[] NoOfLinksArr = new string[SplitQualLinksLen];//declare an array variable that will be used to store all the separated array links
             int SplitQualLinksLenCount = 0;//counter to be used in array when assining different links 
             NameValueCollection[] DiffQualLinks = new NameValueCollection[SplitQualLinksLen];//namevalue collection array that will contain all the links of different quality videos in a array index
           //  NameValueCollection ok2 = new NameValueCollection();
             int p = SplitQualLinks.Length;
            LblForError.Content=p;
             foreach(var a in SplitQualLinks) //foreeach statement cannot be written without curly brackets
              {
                  DiffQualLinks[SplitQualLinksLenCount] = HttpUtility.ParseQueryString(SplitQualLinks[SplitQualLinksLenCount]);
                // NameValueCollection links+SplitQualLinks;
                // ok2 = HttpUtility.ParseQueryString(SplitQualLinks[SplitQualLinksLenCount]);
                // string counter=SplitQualLinksLenCount.ToString();
                // ok.Add(counter, NoOfLinksArr[SplitQualLinksLenCount]);
                  MessageBox.Show(DiffQualLinks[SplitQualLinksLenCount]["type"]);//DiffQualLinks[SplitQualLinksLenCount] contains the complete data related to particular video link from that array we will get the video url
                 SplitQualLinksLenCount++;
              // NameValueCollection  NoOfLinksArrr = HttpUtility.ParseQueryString(SplitQualLinks[0]);
              }
             NameValueCollection FirstLink = HttpUtility.ParseQueryString(SplitQualLinks[0]);
             string Link = FirstLink["url"] + "signature=" + FirstLink["sig"];
             Client.DownloadFile(Link, @"C:\users\sourabh\vstest\vs.mp4");
             }
             catch(Exception ex2)
             {
                 LblForError.Content = "There was an error in downloading the selected video"+ex2;
             }
             
        }


    }
}
