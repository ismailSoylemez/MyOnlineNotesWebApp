
var noteid = -1;

var modalCommenBodyId = "#modal_comment_body";


$(function () {
    //modal show olduktan sonra ilgili conroller altındaki action ı ilgili elemente yükleyecek
    $('#modal_comment').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget);// butonu yakaladım
        noteid = btn.data("note-id");

        $(modalCommenBodyId).load("/Comment/ShowNoteComments/" + noteid);
    })
});



function doComment(btn, e, commentId, spanId) {


    var button = $(btn);
    var mode = button.data("edit-mode");



    //eğer edit moddaysam butonu yeşile çevirmem ikonu tik yapmam gerekiyor
    if (e == "edit_clicked") {

        //ilk tıklamamızda edit mode false ise açmam gerekiyor
        if (!mode) {
            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");
            var btnSpan = button.find("span");
            btnSpan.removeClass("glyphicon-edit");
            btnSpan.addClass("glyphicon-ok");

            $(spanId).addClass("editable"); // eitable true olduğunda css kodlarım devreye girecek

            $(spanId).attr("contenteditable", true);
        }
        //ikinci kez tıkladığımda
        else {
            button.data("edit-mode", false);
            button.addClass("btn-warning");
            button.removeClass("btn-success");
            var btnSpan = button.find("span");
            btnSpan.addClass("glyphicon-edit");
            btnSpan.removeClass("glyphicon-ok");

            $(spanId).removeClass("editable");
            $(spanId).focus();

            $(spanId).attr("contenteditable", false);


            //update sorgusunu çalıştırmam gerekecek

            var txt = $(spanId).text();

            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + commentId,
                data: { text: txt }
            }).done(function (data) {

                //başarılıysa
                if (data.result) {

                    //yorumlar partial tekrar yüklenir
                    $(modalCommenBodyId).load("/Comment/ShowNoteComments/" + noteid);


                }
                else {
                    alert("Yorum Güncellenemedi !");
                }

            }).fail(function () {

                alert("Sunucu ile Bağlantı Kurulamadı !");

            });


        }


    }
    //eğer delete butonuna bastıysam 
    else if (e == "delete_clicked") {

        var dialog_res = confirm("Yorum Silinsin mi ?");

        if (dialog_res == false) {
            return false;
        }
        else {
            $(spanId).focus();

            $.ajax({
                method: "GET",
                url: "/Comment/Delete/" + commentId,
            }).done(function (data) {
                //başarılıysa
                if (data.result) {
                    //yorumlar partial tekrar yüklenir
                    $(modalCommenBodyId).load("/Comment/ShowNoteComments/" + noteid);
                }
                else {
                    alert("Yorum Silinemedi !");
                }

            }).fail(function () {

                alert("Sunucu ile Bağlantı Kurulamadı !");

            });
        }
    }
    //eğer yeni yorum eklersem
    else if (e == "new_clicked") {

        var txt = $('#new_comment_text').val();


        $.ajax({
            method: "POST",
            url: "/Comment/Create/",
            data: { "text": txt, "noteid": noteid }
        }).done(function (data) {
            //başarılıysa
            if (data.result) {
                //yorumlar partial tekrar yüklenir
                $(modalCommenBodyId).load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("Yorum Eklenemedi !");
            }

        }).fail(function () {

            alert("Sunucu ile Bağlantı Kurulamadı !");

        });


    }


}