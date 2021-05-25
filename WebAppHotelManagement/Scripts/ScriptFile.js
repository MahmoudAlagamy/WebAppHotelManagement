

$(document).ready(function () {
    LoadRoomDetails();

    $("#btnSave").click(function () {
        SaveRoomData();
    });


});

function LoadRoomDetails() {
    $.ajax({
        async: true,
        data: 'GET',
        dataType: 'HTML',
        contentType: false,
        processData: false,
        url: '/Room/GetAllRooms',
        success: function (data) {
            $("#divLoadRoomDetails").html(data);
        },
        error: function() {
            alert("There is some Problem to proccess your Request.");
        }
    });
}

function DeleteRoom(roomId) {
    alert(roomId);
}

function EditRoom(roomId) {
    
    $.ajax({
        async: true,
        type: 'GET',
        dataType: 'JSON',
        contentType: 'application/json; charset=utf-8',
        data: { roomId: roomId },
        url: '/Room/EditRoomDetails',
        success: function (data) {
            $("#txtRoomNumber").val(data.RoomNumber);
            $("#txtRoomPrice").val(data.RoomPrice);
            $("#ddBookingStatus").val(data.BookingStatusId);
            $("#ddRoomType").val(data.RoomTypeId);
            $("#txtRoomCapacity").val(data.RoomCapacity);
            $("#txtRoomDescription").val(data.RoomDescription);
            $("#imageRoom").attr('src', "../RoomImages/" + data.RoomImage);
            $("#divAddRoom").modal({ show: true });

        },
        error: function () {
            alert("There is some Problem to proccess your Request.");
        }
    });
}

function SaveRoomData() {
    var formData = new FormData;
    formData.append("RoomNumber", $("#txtRoomNumber").val());
    formData.append("RoomPrice", $("#txtRoomPrice").val());
    formData.append("BookingStatusId", $("#ddBookingStatus").val());
    formData.append("RoomTypeId", $("#ddRoomType").val());
    formData.append("RoomCapacity", $("#txtRoomCapacity").val());
    formData.append("RoomDescription", $("#txtRoomDescription").val());
    formData.append("Image", $("#uploadImage").get(0).files[0]);

    $.ajax({
        async: true,
        type: 'POST',
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data.success === true) {
                alert(data.message);
                ResetRoomInformation();
                LoadRoomDetails();
            }
        },
        error: function () {
            alert("There Is Some Problem To Proccess Your Request Please Try Again Later.");
        }
    });
}

function ResetRoomInformation() {
    $("#txtRoomNumber").val('');
    $("#txtRoomPrice").val('');
    $("#ddBookingStatus").val(1);
    $("#ddRoomType").val(1);
    $("#txtRoomCapacity").val('');
    $("#txtRoomDescription").val('');
    $("#uploadImage").val('');
    $("#txtRoomNumber").focus();
    $("#imageRoom").removeAttr('src');

}

function DisplayImage(result) {

    if (result.files && result.files[0]) {
        var fileReader = new FileReader();
        fileReader.onload = function (e) {
            $("#imageRoom").attr('src', e.target.result);
        }
        fileReader.readAsDataURL(result.files[0]);
    }

}