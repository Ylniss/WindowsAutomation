﻿using System.Reactive.Linq;
using System.Reactive.Subjects;
using WindowsAutomation.Shared.RegularExpression;
using WindowsAutomation.Shared.RegularExpression.Dtos;
using WindowsAutomation.Shared.Web.Downloader.Dtos;

namespace WindowsAutomation.Shared.Web.Downloader;

public class WebDownloader : IWebDownloader
{
    private readonly IRegexExtractor _regexExtractor;
    private readonly HttpClient _httpClient;

    private readonly Subject<string> _whenDownloadStarted = new();
    private readonly Subject<double?> _whenDownloadProgressReceived = new();

    public IObservable<string> WhenDownloadStarted => _whenDownloadStarted.AsObservable();
    public IObservable<double?> WhenDownloadProgressReceived => _whenDownloadProgressReceived.AsObservable();

    public WebDownloader(IRegexExtractor regexExtractor, IMyHttpClientFactory myHttpClientFactory)
    {
        _regexExtractor = regexExtractor;
        _httpClient = myHttpClientFactory.CreateWithProgress(_whenDownloadProgressReceived);
    }

    public async Task<string> DownloadContent(string uri)
    {
        _whenDownloadStarted.OnNext(uri);
        var response = await _httpClient.GetAsync(uri);
        var content = await response.Content.ReadAsStringAsync();

        return content;
    }

    public async Task DownloadFile(WebFileDownload webFileDownload)
    {
        _whenDownloadStarted.OnNext(webFileDownload.Uri);
        var stream = await _httpClient.GetStreamAsync(webFileDownload.Uri);

        var destination = Directory.GetParent(webFileDownload.Destination)!.FullName;
        // if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);

        await using var fileStream = new FileStream(destination, FileMode.OpenOrCreate);
        await stream.CopyToAsync(fileStream);
    }

    public async Task ExtractLinkAndDownloadFile(WebFileDownload webFileDownload, RegexGroupNameMatch regex,
        string prependUri = "")
    {
        var html = await DownloadContent(webFileDownload.Uri);
        var downloadUrl = $"{prependUri}{_regexExtractor.Extract(html, regex)}";

        await DownloadFile(webFileDownload with { Uri = downloadUrl });
    }
}