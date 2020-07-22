using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace GetLotteryData
{
    public partial class lottery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            GetTaiwanLottery(sb);

            //GetHkjc(sb);

            Response.Write(sb.ToString());

        }

        private static void GetTaiwanLottery(StringBuilder sb)
        {
            //539跟大樂透
            const string url = "http://www.taiwanlottery.com.tw/index_new.aspx";

            var client = new HttpClient();
            var responseBody = client.GetStringAsync(url).Result;
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(responseBody);

            //今彩539
            sb.Append("今彩539" + " 開獎結果<br/>");
            var box03 = doc.QuerySelectorAll("div.contents_box03")[0];
            var box03Periods = box03.QuerySelector("div.contents_mine_tx02>span").Text();
            sb.Append("期數：" + box03Periods + "<br/>");
            var box03Lottery = box03.QuerySelectorAll("div.ball_tx.ball_lemon");
            var box03Lottery1 = box03Lottery.Select(s => s.Text()).Take(5).ToArray();
            sb.Append("開出順序：" + string.Join(",", box03Lottery1) + " <br/>");
            var box03Lottery2 = box03Lottery.Select(s => s.Text()).Skip(5).ToArray();
            sb.Append("大小順序：" + string.Join(",", box03Lottery2) + " <br/>");

            sb.Append("<br/>");

            //大樂透
            sb.Append("大樂透" + " 開獎結果<br/>");
            var box02 = doc.QuerySelectorAll("div.contents_box02")[2];
            var box02Periods = box02.QuerySelector(".contents_mine_tx02>span").Text();
            sb.Append("期數：" + box02Periods + "<br/>");
            var box02Lottery = box02.QuerySelectorAll("div.ball_tx.ball_yellow");
            var box02Lottery1 = box02Lottery.Select(s => s.Text()).Take(6).ToArray();
            sb.Append("開出順序：" + string.Join(",", box02Lottery1) + " <br/>");
            var box02Lottery2 = box02Lottery.Select(s => s.Text()).Skip(6).ToArray();
            sb.Append("大小順序：" + string.Join(",", box02Lottery2) + " <br/>");
            var box02Red = box02.QuerySelector("div.ball_red").Text();
            sb.Append("特別號：" + box02Red + " <br/>");

            sb.Append("<br/>");
        }

        private void GetHkjc(StringBuilder sb)
        {
            //六合彩
            const string hkjcurl = "https://bet.hkjc.com/marksix/index.aspx";

            var cookieContainer = new CookieContainer();
            var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip, UseCookies = true, CookieContainer = cookieContainer });
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
            var responseBody = client.GetStringAsync(hkjcurl).Result;
            var parser = new HtmlParser();
            var hkjcdoc = parser.ParseDocument(responseBody);

            sb.Append("六合彩" + " 上期攪珠<br/>");
            var hkjcTable = hkjcdoc.QuerySelectorAll("#oddsTable>table>tbody>tr")[2].QuerySelectorAll("table>tbody>tr")[1].QuerySelectorAll("table>tbody>tr>td")[1];
            var hkjcTablePeriods = hkjcTable.QuerySelector(".content").Text();
            sb.Append("期數：" + hkjcTablePeriods + "<br/>");
            var hkjcLottery = hkjcTable.QuerySelectorAll("tr")[1].QuerySelector("table");

            const string siteurl = "https://bet.hkjc.com";
            sb.Append(hkjcLottery.InnerHtml.Replace("/marksix", siteurl + "/marksix") + "<br/>");

            sb.Append("<br/>");
        }
    }
}