[System.Serializable]
public class EnglishWord 
{
    private string word;
    private string vietnameseTranslation;
    private string pronunciation;
    private string wordType;

    public EnglishWord(string word, string vietnameseTranslation, string pronunciation, string wordType)
    {
        this.word = word;
        this.vietnameseTranslation = vietnameseTranslation;
        this.pronunciation = pronunciation;
        this.wordType = wordType;
    }
    public string getWord()
    {
        return this.word;
    }
    public string getVietnameseTranslation()
    {
        return this.vietnameseTranslation;
    }
    public string getPronunciation()
    {
        return this.pronunciation;
    }
    public string getWordType()
    {
        return this.wordType;
    }

}