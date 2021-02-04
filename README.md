# TwtScrap

A console app that scrapes tweets from Twitter
Specifically built to scrape London Underground/Tube Status for data analysis practice

## Features

* Auto mode: Input a twitter url then it is going to scrape all the tweets and save them to a csv file
* Manual mode: Load in a html file that contains the tweets that is needed to scrap, the console app is going to save them to a csv file
* Save all the tweet messages and the published DateTime
* Remove all the new line characters and symbols that can cause confusion for the csv and to read

## Screenshots

![screenshot_project](screenshot_project.png)

![screenshot_csv](screenshot_csv.png)

## Tech

* Built with C# .NET Framework 4.6.1
* Selenium 3.7.0
* Fizzler 1.1.0
* HtmlAgilityPack 1.6.6

## Background

One day on our way to work, my flatmate who is a data analyst and I a software developer was stuck in the London Underground due to minor delays. The delays happen so often that we were thinking maybe we can try to predict the next delay by looking at some correlations and patterns. The data part was probably a bit too ambitious for us at that time so by no surprise it didn't go anywhere, however before that we need a way to get data of the Tube and I realised the easiest way is to scrape them from Twitter as each Tube line has its own Twitter account to update the public about different delays. So here is the Twitter Scrapper that I wrote and used at that time. It is not much but at least I have actually found it useful. 