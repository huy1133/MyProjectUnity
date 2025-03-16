using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] Text TagName;
    [SerializeField] Text textPlay;
    [SerializeField] Text textDisplayNumber;
    [SerializeField] AudioClip takeClip;
    [SerializeField] Slider slider;
    [SerializeField] Image mainImage;
    [SerializeField] GameObject cardPanel;
    [SerializeField] Image card;
    [SerializeField] Text cardText;
    [SerializeField] Sprite[] sprites;


    List<string>[] DataDare = new List<string>[] {null, null};
    int lenght;
    int currentUsedNumber;
    bool canTake;
    AudioSource takeSound;
    AudioSource audioSource;

    private void Awake()
    {
        slider.value = 1;
    }

    void Start()
    {
        StartCoroutine(ChangeConstantlyFontTag());
        CreateData();
        lenght = DataDare[0].Count + DataDare[1].Count;
        currentUsedNumber = 0;
        audioSource = GetComponent<AudioSource>();
        takeSound = gameObject.AddComponent<AudioSource>();
        takeSound.clip = takeClip;
        canTake = true;
        cardPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        textDisplayNumber.text = currentUsedNumber+"/ "+lenght+"\n"+"lá bài được rút";
        audioSource.volume = slider.value;
    }


    IEnumerator ChangeConstantlyFontTag()
    {
        int k = 1;
        int type = 1;
        while (true)
        {
            TagName.fontSize += k * type;
            textPlay.fontSize += k * type;
            if (TagName.fontSize ==100)
            {
                type = 1;
            }
            if (TagName.fontSize == 130)
            {
                type = -1;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void takeCard()
    {
        if (canTake)
        {
            takeSound.Play();
            currentUsedNumber++;
            canTake = false;
            StartCoroutine(TakingCard());
            while (true)
            {
                int type = Random.Range(1, 11);
                type %= 2;
                if (DataDare[type].Count != 0)
                {
                    int index = Random.Range(0, DataDare[type].Count);
                    cardText.text = DataDare[type][index];
                    DataDare[type].RemoveAt(index);
                    int cardType = index % 7;
                    card.sprite = sprites[cardType];
                    break;
                }
            }
        }
        if(currentUsedNumber == lenght)
        {
            mainImage.gameObject.SetActive(false);
        }
    }

    IEnumerator TakingCard()
    {
        int k = 0;
        while (k <= 360)
        {
            mainImage.rectTransform.rotation = Quaternion.Euler(0, k, 0);
            k+=4;
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.2f);
        cardPanel.SetActive(true);
    }

    public void CloseCard()
    {
        canTake = true;
        cardPanel.SetActive(false);
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(0);
    }

    void CreateData()
    {
        DataDare[0] = new List<string> {
            "Gọi điện cho người cuối cùng nhắn tin cho bạn và nói 'Tôi nhớ bạn'.",
            "Nhảy một bài nhảy TikTok mà mọi người chọn.",
            "Đọc to tin nhắn gần nhất trong điện thoại của bạn.",
            "Bắt chước giọng nói của một người trong nhóm.",
            "Tạo dáng kỳ lạ và giữ nguyên trong 1 phút.",
            "Hát một bài hát theo phong cách opera.",
            "Kể bí mật hài hước nhất mà bạn từng giữ.",
            "Nhắn tin cho crush và nói 'Tớ có chuyện quan trọng muốn nói với cậu'.",
            "Chơi oẳn tù tì với người bên trái, ai thua phải uống.",
            "Nói một câu tán tỉnh với người đối diện.",
            "Giữ thăng bằng trên một chân trong 30 giây.",
            "Nhảy lò cò một vòng quanh phòng.",
            "Ăn một thìa nước mắm hoặc tương ớt.",
            "Đọc một đoạn rap một cách nghiêm túc.",
            "Tạo một bài thơ ngẫu nhiên trong vòng 30 giây.",
            "Giả vờ làm một con mèo trong một phút.",
            "Làm mặt xấu nhất có thể và giữ trong 10 giây.",
            "Đứng im như tượng trong 2 phút.",
            "Hát một bài hát nhưng thay toàn bộ lời bằng từ 'meo meo'.",
            "Đi vòng quanh phòng như một con vịt.",
            "Nhắn tin cho bố/mẹ và hỏi 'Bố/mẹ có tin vào người ngoài hành tinh không?'.",
            "Đọc to một tin nhắn cũ từ năm ngoái.",
            "Gọi điện cho ai đó và hát Happy Birthday.",
            "Nhảy sexy dance trong 30 giây.",
            "Đọc một câu chuyện đáng sợ bằng giọng đáng sợ nhất có thể.",
            "Giả vờ là một nhân vật hoạt hình trong 2 phút.",
            "Làm một động tác yoga khó trong 30 giây.",
            "Nói một câu chuyện nhưng không được dùng chữ 'và'.",
            "Nói một câu tỏ tình thật lãng mạn với người bên trái.",
            "Kể một bí mật mà bạn chưa từng nói với ai.",
            "Hát một bài hát mà bạn ghét nhất.",
            "Nhảy một điệu nhảy điên rồ trong 30 giây.",
            "Bắt chước giọng nói của một người nổi tiếng.",
            "Chơi một trò chơi đoán chữ trong 1 phút.",
            "Đọc một tin nhắn cũ mà bạn thấy xấu hổ.",
            "Giả vờ là một con chó trong 1 phút.",
            "Kể một câu chuyện cười siêu nhạt nhẽo.",
            "Nói một câu chuyện hoàn toàn bịa đặt nhưng thuyết phục như thật.",
            "Giữ nguyên tư thế plank trong 30 giây.",
            "Hát một bài hát bằng giọng điệu nghiêm túc nhất có thể.",
            "Nói một câu chuyện mà chỉ có 5 từ.",
            "Bắt chước dáng đi của một con vật mà bạn yêu thích.",
            "Hát một bài hát thiếu nhi.",
            "Nhắn tin cho ai đó và nói 'Tớ cần nói chuyện với cậu gấp!'.",
            "Tự tạo ra một điệu nhảy ngẫu nhiên và biểu diễn.",
            "Nói điều hài hước nhất mà bạn từng nghe.",
            "Giả vờ là một streamer nổi tiếng trong 1 phút.",
            "Nói giọng trẻ con trong 2 phút.",
            "Hát một bài hát theo phong cách metal.",
            "Nhảy múa như một người ngoài hành tinh.",
            "Hát một bài hát theo phong cách rock.",
            "Nhảy như một con gà trong 30 giây.",
            "Đọc to tên 5 người bạn thân nhất của bạn mà không suy nghĩ.",
            "Gọi điện cho ai đó và nói rằng bạn vừa trúng số.",
            "Giữ hơi trong 15 giây và sau đó hét thật to.",
            "Làm mặt hài hước nhất có thể và giữ trong 10 giây.",
            "Bắt chước cách đi của một người nổi tiếng.",
            "Nói một câu chuyện bằng giọng điệu của một người say.",
            "Nhảy điệu moonwalk như Michael Jackson.",
            "Tạo ra một câu chuyện hài hước ngẫu nhiên trong vòng 1 phút.",
            "Giả vờ đang chơi đàn guitar điện mà không có đàn.",
            "Chơi oẳn tù tì với người bên phải, ai thua phải làm mặt xấu.",
            "Hít đất 10 lần ngay bây giờ.",
            "Nhảy lên xuống liên tục trong 20 giây.",
            "Giữ tư thế squat trong 30 giây.",
            "Hát một bài hát theo phong cách rap.",
            "Đi lùi quanh phòng trong 15 giây.",
            "Tạo một câu chuyện chỉ với 5 từ.",
            "Hát một bài hát mà bạn không thích.",
            "Nhảy theo điệu của một bài hát bất kỳ.",
            "Nói một câu chuyện mà không sử dụng chữ 'và'.",
            "Giả vờ là một robot trong 1 phút.",
            "Hát một bài hát bằng giọng của một nhân vật hoạt hình.",
            "Nói một câu tán tỉnh với một người bất kỳ.",
            "Đi một vòng quanh phòng như một con vịt.",
            "Bắt chước giọng một nhân vật nổi tiếng.",
            "Nhắn tin cho ai đó và nói 'Tớ thích cậu'.",
            "Nhảy theo một bài TikTok nổi tiếng.",
            "Đọc to đoạn chat gần nhất của bạn.",
            "Nói ra bí mật mà bạn chưa từng nói với ai.",
            "Thử chồng 5 đồ vật lên nhau mà không làm rơi.",
            "Giả vờ là một streamer nổi tiếng trong 1 phút.",
            "Tạo một điệu nhảy mới ngay lập tức.",
            "Hát một bài hát nhưng chỉ bằng tiếng 'meo meo'.",
            "Hóa thân thành một con khỉ trong 30 giây.",
            "Nói nhanh tên 10 loài động vật trong 10 giây.",
            "Bắt chước dáng đi của một ông già.",
            "Nhảy múa như một người ngoài hành tinh.",
            "Nói một câu chuyện đáng sợ theo phong cách hài hước.",
            "Tạo dáng giống một bức tượng trong 1 phút.",
            "Đi lùi trong 30 giây mà không vấp ngã.",
            "Giả vờ là một ca sĩ nổi tiếng đang biểu diễn.",
            "Làm động tác plank trong 20 giây.",
            "Nói một câu chuyện nhưng chỉ bằng từ ngữ của trẻ con.",
            "Nhảy điệu cha cha cha ngay bây giờ.",
            "Nói một câu chuyện không có thật mà nghe cực kỳ thuyết phục.",
            "Hát một bài hát theo phong cách opera.",
            "Nhắn tin cho bố mẹ và hỏi 'Bố/mẹ có tin vào UFO không?'.",
            "Diễn tả một bộ phim nổi tiếng mà không nói tên, để mọi người đoán.",
            "Điều khiển một người khác như một con rối trong 1 phút.",
            "Hát một bài hát chỉ bằng âm thanh 'bíp bíp'.",
            "Bắt chước cách nói chuyện của một nhân vật hoạt hình nổi tiếng.",
            "Diễn xuất như bạn đang ở trong một bộ phim hành động.",
            "Đi vòng quanh phòng như thể bạn đang đi trên dây.",
            "Nói một câu chuyện nhưng thay tất cả nguyên âm bằng 'o'.",
            "Nhảy điệu salsa với một người bất kỳ.",
            "Tạo dáng như một người mẫu chuyên nghiệp trong 30 giây.",
            "Đọc to một tin nhắn gần đây nhất của bạn mà không xem trước.",
            "Làm động tác yoga ngẫu nhiên trong 20 giây.",
            "Tạo ra một câu chuyện đáng sợ nhưng chỉ sử dụng 10 từ.",
            "Bắt chước một nhân vật trong trò chơi điện tử yêu thích của bạn.",
            "Diễn lại một cảnh kinh điển trong bộ phim bạn thích.",
            "Tạo một bài thơ ngẫu nhiên trong 30 giây.",
            "Chơi một nhạc cụ tưởng tượng và làm như đang biểu diễn.",
            "Giả vờ bạn đang trượt băng trong 15 giây.",
            "Nói tên 5 bài hát có màu sắc trong tiêu đề.",
            "Nhảy theo một bài hát nhưng không được nghe nhạc.",
            "Tạo ra một tiếng động vật không có thật.",
            "Nói nhanh tên 5 bộ phim mà không dừng lại.",
            "Hóa thân thành một nhân vật phản diện trong phim.",
            "Thực hiện một bài tập thể dục ngẫu nhiên trong 30 giây.",
            "Giả vờ như bạn vừa trúng số độc đắc.",
            "Nói một câu thật buồn theo cách hài hước.",
            "Tạo ra một điệu nhảy mới ngay bây giờ.",
            "Điều khiển một chiếc ghế như thể nó là một chiếc xe hơi.",
            "Diễn xuất như bạn vừa phát hiện ra một bí mật động trời.",
            "Đọc một đoạn văn với giọng nói của một cụ già.",
            "Bắt chước giọng của bạn khi bị cảm lạnh.",
            "Kể lại một câu chuyện nhưng thay đổi toàn bộ kết thúc.",
            "Hát một bài hát nhưng thay lời bằng tiếng động vật.",
            "Nhảy một điệu nhảy của một nền văn hóa khác.",
            "Tạo ra một cuộc phỏng vấn giả tưởng với người nổi tiếng.",
            "Diễn tả một nghề nghiệp mà không dùng lời nói.",
            "Nói một câu chuyện bằng cách chỉ sử dụng hành động.",
            "Diễn xuất như thể bạn đang bay trên trời.",
            "Tạo ra một cuộc tranh cãi giả tưởng với chính mình.",
            "Diễn như thể bạn là một nhân vật trong truyện cổ tích.",
            "Nói một câu chuyện nhưng thay mọi danh từ bằng một từ mới.",
            "Làm như bạn đang chơi một nhạc cụ vô hình.",
            "Thực hiện một bài thể dục nhưng phải cười suốt.",
            "Hát một bài hát nổi tiếng nhưng theo điệu hát ru.",
            "Đọc to bảng chữ cái nhưng thay thế chữ 'A' bằng tiếng chó sủa.",
            "Nói một câu chuyện nhưng chỉ dùng dấu chấm câu để diễn đạt.",
            "Hát một bài hát bằng giọng robot.",
            "Thực hiện một cuộc gọi giả tưởng với một người ngoài hành tinh.",
            "Đi quanh phòng như thể bạn đang trên mặt trăng.",
            "Diễn xuất như bạn vừa biến thành một con mèo trong 30 giây.",
        };

        DataDare[1] = new List<string> {
             "Uống một ngụm nếu bạn đang đeo đồng hồ.",
            "Uống hai ngụm nếu bạn từng gửi tin nhắn cho người yêu cũ.",
            "Uống nếu bạn đang mặc áo trắng.",
            "Uống ba ngụm nếu bạn đã từng say đến mức không nhớ gì.",
            "Uống một ngụm nếu bạn đã từng ngủ gật trong lớp hoặc khi họp.",
            "Uống nếu bạn đã từng giả vờ say để tránh trách nhiệm.",
            "Uống nếu bạn từng chụp ảnh selfie trong gương hôm nay.",
            "Uống ba ngụm nếu bạn có ít hơn 50% pin điện thoại ngay lúc này.",
            "Uống một ngụm nếu bạn từng quên sinh nhật bạn thân.",
            "Uống hai ngụm nếu bạn từng đăng trạng thái 'buồn' để gây sự chú ý.",
            "Uống nếu bạn đã từng khóa tài khoản Facebook một lần.",
            "Uống ba ngụm nếu bạn từng quên mật khẩu quan trọng.",
            "Uống một ngụm nếu bạn đã từng đi làm muộn.",
            "Uống nếu bạn đã từng bị từ chối lời tỏ tình.",
            "Uống hai ngụm nếu bạn từng thích hai người cùng một lúc.",
            "Uống một ngụm nếu bạn từng ngã ở nơi công cộng.",
            "Uống nếu bạn từng mất đồ khi đi du lịch.",
            "Uống ba ngụm nếu bạn từng quên ví khi ra ngoài.",
            "Uống một ngụm nếu bạn đang đeo kính.",
            "Uống hai ngụm nếu bạn từng cãi nhau với bạn thân.",
            "Uống một ngụm nếu bạn từng trốn học hoặc trốn làm.",
            "Uống nếu bạn đã từng ăn hết cả một túi snack trong một lần.",
            "Uống ba ngụm nếu bạn từng lỡ hẹn một cuộc gặp quan trọng.",
            "Uống nếu bạn từng gửi tin nhắn nhầm người.",
            "Uống một ngụm nếu bạn đang mặc quần jean.",
            "Uống hai ngụm nếu bạn từng xem phim mà khóc.",
            "Uống nếu bạn từng thích ai đó mà không dám nói.",
            "Uống ba ngụm nếu bạn từng ngủ quên trong lúc gọi điện.",
            "Uống một ngụm nếu bạn đã từng bị gọi là 'đồ ngốc'.",
            "Uống một ngụm nếu bạn đang đeo đồng hồ.",
            "Uống hai ngụm nếu bạn từng gửi tin nhắn nhầm người.",
            "Uống nếu bạn đang mặc áo màu đen.",
            "Uống ba ngụm nếu bạn đã từng ngủ quên khi nhắn tin.",
            "Uống một ngụm nếu bạn từng nói dối để tránh một cuộc hẹn.",
            "Uống nếu bạn từng giả vờ say để không làm gì đó.",
            "Uống nếu bạn đã từng khóa tài khoản mạng xã hội.",
            "Uống ba ngụm nếu bạn có ít hơn 30% pin điện thoại ngay lúc này.",
            "Uống một ngụm nếu bạn từng quên sinh nhật ai đó.",
            "Uống hai ngụm nếu bạn từng xem phim mà khóc.",
            "Uống nếu bạn đã từng bị từ chối lời tỏ tình.",
            "Uống hai ngụm nếu bạn từng thích hai người cùng một lúc.",
            "Uống một ngụm nếu bạn từng vấp ngã ở nơi công cộng.",
            "Uống nếu bạn từng mất điện thoại ít nhất một lần.",
            "Uống ba ngụm nếu bạn từng quên ví khi ra ngoài.",
            "Uống một ngụm nếu bạn đang đeo kính.",
            "Uống hai ngụm nếu bạn từng trốn học hoặc trốn làm.",
            "Uống nếu bạn từng gửi tin nhắn mà ngay lập tức hối hận.",
            "Uống nếu bạn đang mặc quần jean.",
            "Uống ba ngụm nếu bạn từng ngủ quên khi gọi điện.",
            "Uống một ngụm nếu bạn từng bị ai đó gọi là 'lười'.",
            "Uống nếu bạn từng lỡ hẹn một cuộc gặp quan trọng.",
            "Uống ba ngụm nếu bạn đã từng quên ngày kỷ niệm.",
            "Uống nếu bạn từng giả vờ bận để tránh ai đó.",
            "Uống hai ngụm nếu bạn từng lỡ chuyến xe buýt hoặc tàu.",
            "Uống nếu bạn từng làm mất một món đồ quan trọng.",
            "Uống ba ngụm nếu bạn từng khóa nhầm cửa khi còn chìa khóa bên trong.",
            "Uống nếu bạn từng quên làm bài tập hoặc công việc quan trọng.",
            "Uống một ngụm nếu bạn đã từng nói dối hôm nay.",
            "Uống một ngụm nếu bạn từng ngủ quên khi đang xem phim.",
            "Uống hai ngụm nếu bạn đã từng đi muộn hơn 30 phút.",
            "Uống nếu bạn từng quên làm bài tập hoặc công việc.",
            "Uống ba ngụm nếu bạn từng lỡ mất một chuyến bay hoặc xe buýt.",
            "Uống một ngụm nếu bạn từng gọi nhầm tên ai đó.",
            "Uống nếu bạn từng đi ra khỏi nhà rồi quên mang theo ví.",
            "Uống ba ngụm nếu bạn từng bị mắc kẹt trong thang máy.",
            "Uống một ngụm nếu bạn từng gửi nhầm tin nhắn quan trọng.",
            "Uống hai ngụm nếu bạn từng lỡ dịp sinh nhật quan trọng.",
            "Uống nếu bạn từng quên mật khẩu quan trọng.",
            "Uống nếu bạn từng bị ai đó đọc trộm tin nhắn.",
            "Uống ba ngụm nếu bạn từng mất điện thoại.",
            "Uống một ngụm nếu bạn từng nhắn tin cho người lạ rồi nhận ra.",
            "Uống nếu bạn từng bị ai đó troll trên mạng xã hội.",
            "Uống ba ngụm nếu bạn từng đăng nhầm bài viết trên mạng.",
            "Uống hai ngụm nếu bạn từng trượt môn hoặc bị điểm kém.",
            "Uống nếu bạn từng phải gọi hỗ trợ vì khóa xe trong ô tô.",
            "Uống ba ngụm nếu bạn từng bị ai đó nhìn thấy lúc đang nói một mình.",
            "Uống một ngụm nếu bạn từng ngủ quên trong lớp hoặc cuộc họp.",
            "Uống nếu bạn từng nhầm người khi chào hỏi.",
            "Uống hai ngụm nếu bạn từng đi nhầm vào nhà vệ sinh khác giới.",
            "Uống nếu bạn từng làm rơi điện thoại xuống nước.",
            "Uống ba ngụm nếu bạn từng quên ngày kỷ niệm quan trọng.",
            "Uống một ngụm nếu bạn từng nói dối chỉ để tránh đi chơi.",
            "Uống nếu bạn từng làm đổ đồ uống lên người ai đó.",
            "Uống ba ngụm nếu bạn từng bị ai đó bắt gặp đang nhảy một mình.",
            "Uống nếu bạn từng bị giật điện nhẹ.",
            "Uống hai ngụm nếu bạn từng làm hỏng một món đồ mượn từ người khác.",
            "Uống nếu bạn từng bị mất xe hoặc chìa khóa xe.",
        };
    }
}
