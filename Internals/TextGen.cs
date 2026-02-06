using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SolyankaGuide.Internals
{


    internal class TextGen
    {

        private static readonly SolidColorBrush spoilerColor = (SolidColorBrush) new BrushConverter().ConvertFromString("#1a1a1a")!;

        public static TextBlock GetText(string[] text, bool centered, double width)
        {
            var tb = new TextBlock
            {
                Width = width,
                Foreground = Brushes.White,
                FontSize = 24,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = centered ? TextAlignment.Center : TextAlignment.Left,
                Focusable = false,
                Margin = new Thickness(0, 5, 0, 0)
            };
            tb.SizeChanged += (s, e) =>
            {
                foreach (var inline in tb.Inlines)
                {
                    if (inline == null) continue;
                    if (inline is not InlineUIContainer ic) continue;
                    if (ic.Child is not Grid g) continue;
                    if (g.Children.Count == 0 || g.Children[0] is not Image img) continue;
                    g.Width = tb.Width;
                    img.Width = tb.ActualWidth / 3;
                }
            };
            string[] lines = text;
            for (int i = 0; i < lines.Length; i++)
            {
                if (i != 0) tb.Inlines.Add("\n");
                var line = lines[i];
                bool hasHyper = Hyperlink(line, out string output, out string link, out string words);
                bool hasSpoiler = Spoiler(line, out string spoilerOutput, out string spoilerWords);
                bool isImage = ImageInline(line, out InlineUIContainer? container);
                if (isImage)
                {
                    tb.Inlines.Add(container);
                }
                else if (hasHyper)
                {
                    int markerIndex = output.IndexOf(words);
                    string before = output[..markerIndex];
                    string after = output[(markerIndex + words.Length)..];
                    var textBlock = new TextBlock
                    {
                        TextWrapping = TextWrapping.Wrap
                    };
                    if (!string.IsNullOrEmpty(before))
                        textBlock.Inlines.Add(new Run(before + " "));
                    var hyperlink = new Hyperlink(new Run(words))
                    {
                        Foreground = Brushes.Aqua,
                        TextDecorations = TextDecorations.Underline,
                    };
                    hyperlink.Click += (s, e) =>
                    {
                        UrlOpener.OpenUrl(link);
                    };
                    textBlock.Inlines.Add(hyperlink);
                    if (!string.IsNullOrEmpty(after))
                        textBlock.Inlines.Add(new Run(after));
                    tb.Inlines.Add(textBlock);
                }
                else if (hasSpoiler)
                {
                    int markerIndex = spoilerOutput.IndexOf(spoilerWords);
                    string before = spoilerOutput[..markerIndex];
                    string after = spoilerOutput[(markerIndex + spoilerWords.Length)..];
                    var textBlock = new TextBlock
                    {
                        TextWrapping = TextWrapping.Wrap
                    };
                    if (!string.IsNullOrEmpty(before))
                        textBlock.Inlines.Add(new Run(before + " "));
                    var hyperlink = new Hyperlink(new Run(spoilerWords))
                    {
                        Foreground = spoilerColor,
                        Background = spoilerColor,
                        TextDecorations = null,
                        Cursor = Cursors.Arrow
                    };
                    hyperlink.Click += (s, e) =>
                    {
                        hyperlink.Inlines.FirstInline.Foreground = Brushes.White;
                        hyperlink.Inlines.FirstInline.Background = Brushes.Transparent;
                    };
                    hyperlink.MouseEnter += (s, e) =>
                    {
                        hyperlink.Foreground = Brushes.Gray;
                        hyperlink.Background = Brushes.Gray;
                    };
                    hyperlink.MouseLeave += (s, e) =>
                    {
                        hyperlink.Foreground = spoilerColor;
                        hyperlink.Background = spoilerColor;
                    };
                    textBlock.Inlines.Add(hyperlink);
                    if (!string.IsNullOrEmpty(after))
                        textBlock.Inlines.Add(new Run(after));
                    tb.Inlines.Add(textBlock);
                }
                else
                {
                    tb.Inlines.Add(line);
                }

            }
            return tb;

        }

        private static bool ImageInline(string input, out InlineUIContainer? output)
        {
            string pattern = @"%img%(.*?)%eimg%";
            Regex regex = new(pattern);
            Match match = regex.Match(input);
            if (match.Success)
            {
                string firstGroup = match.Groups[1].Value;
                output = new InlineUIContainer(new Grid
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Children =
                    {
                        new Image {
                            Margin = new Thickness(0, 5, 0, 5),
                            Source = ImageLoader.LoadImage(firstGroup),
                            HorizontalAlignment = HorizontalAlignment.Center
                        }
                    }
                });
                return true;
            }
            output = null;
            return false;
        }

        private static bool Spoiler(string input, out string output, out string wordsToReplace)
        {
            string pattern = @"%s%(.*?)%es%";
            Regex regex = new(pattern);
            Match match = regex.Match(input);
            if (match.Success)
            {
                string firstGroup = match.Groups[1].Value;
                output = input.Replace("%es%", "").Replace($"%s%", "");
                wordsToReplace = firstGroup;
                return true;
            }
            wordsToReplace = "";
            output = input;
            return false;
        }

        private static bool Hyperlink(string input, out string output, out string link, out string wordsToReplace)
        {
            string pattern = @"%l=(.*?)%(.*?)%el%";
            Regex regex = new(pattern);
            Match match = regex.Match(input);
            if (match.Success)
            {
                string firstGroup = match.Groups[1].Value;
                string secondGroup = match.Groups[2].Value;
                output = input.Replace("%el%", "").Replace($"%l={firstGroup}%", "");
                wordsToReplace = secondGroup;
                link = firstGroup;
                return true;
            }
            link = "";
            wordsToReplace = "";
            output = input;
            return false;
        }
    }
}
