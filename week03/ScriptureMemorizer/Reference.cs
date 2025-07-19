public class Reference
{
    private string _book;
    private int _chapter;
    private int _verseStart;
    private int _verseEnd;

    public Reference(string referenceText)
    {
        
        var parts = referenceText.Split(' ');
        _book = parts[0];
        var chapterAndVerses = parts[1].Split(':');
        _chapter = int.Parse(chapterAndVerses[0]);

        if (chapterAndVerses[1].Contains("-"))
        {
            var verses = chapterAndVerses[1].Split('-');
            _verseStart = int.Parse(verses[0]);
            _verseEnd = int.Parse(verses[1]);
        }
        else
        {
            _verseStart = int.Parse(chapterAndVerses[1]);
            _verseEnd = _verseStart;
        }
    }

    public string GetDisplayText()
    {
        return _verseStart == _verseEnd
            ? $"{_book} {_chapter}:{_verseStart}"
            : $"{_book} {_chapter}:{_verseStart}-{_verseEnd}";
    }
}
