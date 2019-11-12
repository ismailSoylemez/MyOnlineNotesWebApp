
//ilk olarak sayfadaki notların id sini çekmem gerekecek bunun için biz dizi tanımlayıp sayfada 'div[data-note-id]' properties özellikli nesneyi alıyorum. daha sonra onu jquery nesnesine çeviriyorum ve diziye ekliyorum. 
$(function () {
    var noteids = [];

    $("div[data-note-id]").each(function (i, e) {
        noteids.push($(e).data("note-id"));
    });

    console.log(noteids);


    $.ajax({
        method: "POST",
        url: "/Note/GetLiked",
        data: { ids: noteids }
    }).done(function (data) {
        console.log(data);
        if (data.result != null && data.result.length > 0) {
            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i]; // tek tek id leri alıyoruz
                var likedNote = $("div[data-note-id=" + id + "]");// sayfada buluna idleri tespit ediyorum
                var btn = likedNote.find("button[data-liked]"); // like butonum
                var span = btn.children().first(); // yıldız yapacağım span yani like atılmış not.

                btn.data("liked", true);
                //?????????????????????


                span.removeClass("glyphicon-star-empty");
                span.addClass("glyphicon-star");
            }
        }
    }).fail(function () {


    });




    // beğeni butonuna basıldığında olacaklar
    $("button[data-liked]").click(function () {

        var btn = $(this); //butonu elde ettim
        var liked = btn.data("liked"); //işlem yapılmadan önceki like durumu
        var noteid = btn.data("note-id");
        var spanStar = btn.find("span.like-star");
        var spanCount = btn.find("span.like-count");

        console.log("liked:" + liked);
        console.log("like count : " + spanCount.text());


        $.ajax({
            method: "POST",
            url: "/Note/SetLikeState",
            data: { "noteid": noteid, "liked": !liked } // false ise true veya tam tersi olacak // like ı ters yolladık
        }).done(function (data) {

            console.log("data:" + data);

            if (data.hasError == true) {
                alert(data.errorMessage);
            }
            else {

                liked = !liked; //liked ı normale çeviriyorum
                btn.data("liked", liked); //attribute u güncelledim yeni değer ile
                spanCount.text(data.result); // yeni beğeni sayımı güncelledim (controllerdan yolladığımız değerdir)

                console.log("like count(after) : " + spanCount.text());


                spanStar.removeClass("glyphicon-star-empty");
                spanStar.removeClass("glyphicon-star");

                if (liked) {
                    spanStar.addClass("glyphicon-star");
                } else {
                    spanStar.addClass("glyphicon-star-empty");
                }
            }
        }).fail(function () {
            alert("sunucu ile bağlantı kurulamadı !");
        });



    });











});







