﻿using Finance_WebScraper.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Finance_WebScraper.Services
{
    public class Scraper
    {
        public List<Stock> Scrape()
        {
            // Add --headless to options 
            var options = new ChromeOptions();
            options.AddArguments("--headless");
            options.AddArguments("--disable-gpu");
            options.AddArguments("disable-popup-blocking");

            // Initial new ChromeDriver and navigate to Yahoo URL
            var chromeDriver = new ChromeDriver("C:\\Users\\Sreenath\\source\\repos\\Finance_WebScraper", options);

            chromeDriver.Navigate().GoToUrl("https://login.yahoo.com");
            chromeDriver.Manage().Window.Maximize();

            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            // Input username and click on Next 
            chromeDriver.FindElementById("login-username").SendKeys("surekha.srinivasan@yahoo.com");
            chromeDriver.FindElementById("login-signin").Click();

            // Input password and click on submit
            chromeDriver.FindElementById("login-passwd").SendKeys("Careerdevs");
            chromeDriver.FindElementById("login-signin").Click();


            // After password verification navigate to Yahoo portfolio page

            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            chromeDriver.Url = "https://finance.yahoo.com/portfolio/p_0/view/v1";

            //var closePopup = chromeDriver.FindElementByXPath("//dialog[@id = '__dialog']/section/button");
            //closePopup.Click();

            //var stocks = chromeDriver.FindElements(By.XPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[*]/td[*]"));
            //foreach (var stock in stocks)
            //    Console.WriteLine(stock.Text);

            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            IWebElement list = chromeDriver.FindElementByTagName("tbody");
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> stocks = list.FindElements(By.TagName("tr"));
            int count = stocks.Count();

            List<Stock> stockList = new List<Stock>();
            for (int i = 1; i <= count; i++)
            {
                var symbol = chromeDriver.FindElementByXPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[1]").GetAttribute("innertext");
                var lastPrice = chromeDriver.FindElementByXPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[2]").GetAttribute("innerText");
                var change = chromeDriver.FindElementByXPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[3]").GetAttribute("innerText");
                var percentChange = chromeDriver.FindElementByXPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[4]").GetAttribute("innerText");
                var currency = chromeDriver.FindElementByXPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[5]").GetAttribute("innerText");
                var marketCap = chromeDriver.FindElementByXPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[13]").GetAttribute("innerText");

                //var symbol = chromeDriver.FindElementByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[1]/span/a").GetAttribute("innerText");
                //var lastPrice = chromeDriver.FindElementByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[2]/span").GetAttribute("innerText");
                //var change = chromeDriver.FindElementByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[3]/span").GetAttribute("innerText");
                //var percentChange = chromeDriver.FindElementByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[4]/span").GetAttribute("innerText");
                //var currency = chromeDriver.FindElementByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[5]").GetAttribute("innerText");
                //var marketCap = chromeDriver.FindElementByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[13]/span").GetAttribute("innerText");

                Stock stock = new Stock();
                stock.Symbol = symbol;
                stock.LastPrice = Decimal.Parse(lastPrice);
                stock.Change = Decimal.Parse(change);
                //stock.PercentChange = Decimal.Parse(percentChange);
                stock.PercentChange = Decimal.Parse(percentChange.Trim('%'));
                stock.Currency = currency;
                stock.MarketCap = marketCap;

                stockList.Add(stock);
            }
            //chromeDriver.Quit();           
            return stockList;
        }        
    }
}