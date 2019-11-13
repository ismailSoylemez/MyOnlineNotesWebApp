
    $(function () {
        //modal show olduktan sonra ilgili conroller altındaki action ı ilgili elemente yükleyecek
        $('#modal_noteDetail').on('show.bs.modal', function (e) {

            var btn = $(e.relatedTarget);// butonu yakaladım
            noteid = btn.data("note-id");

            $("#modal_noteDetail_body").load("/Note/GetNoteText/" + noteid);
        })
    });