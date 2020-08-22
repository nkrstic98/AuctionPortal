// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var connection =  new signalR.HubConnectionBuilder().withUrl("/update").build();

function handleError(error) {
    alert(error);
}

connection.start().catch(handleError);

function filter() {
    var auctionName = $("#auctionName").val();
    var minPrice = $("#minPrice").val();
    var maxPrice = $("#maxPrice").val();
    var auctionState = $("#auctionState").val();
    var verificationToken = $("input[name='__RequestVerificationToken']").val();

    $.ajax({
        type: "POST",
        url: "/Auction/Search",
        data: {
            "name": auctionName,
            "minPrice": minPrice,
            "maxPrice": maxPrice,
            "state": auctionState,
            "__RequestVerificationToken": verificationToken
        },
        dataType: "text",
        success: function(response) {
            for(let i = 0; i < intervals.length; i++) {
                clearInterval(intervals[i].interval);
            }
            intervals = [];
            $("#auctions").html(response);
        },
        error: function(response) {
            alert(response);
        }
    }
    )
}

function closeAuction(auctionId) {
    var verificationToken = $("input[name='__RequestVerificationToken']").val();

    $.ajax({
        type: "POST",
        url: "/Auction/closeAuction",
        data: {
            "auctionId": auctionId,
            "__RequestVerificationToken": verificationToken
        },
        dataType: "text",
        success: function(response) {
            for(let i = 0; i < intervals.length; i++) {
                if(intervals[i].auctionId == auctionId) {
                    clearInterval(intervals[i].interval);
                }
            }
            id = "#auction" + auctionId;
            $(id).html(response);
        },
        error: function(response) {
            alert(response);
        }
    })
}

function noFilters() {
    var verificationToken = $("input[name='__RequestVerificationToken']").val();

    $.ajax({
        type: "POST",
        url: "/Auction/Search",
        data: {
            "name": "",
            "minPrice": "",
            "maxPrice": "",
            "state": "",
            "__RequestVerificationToken": verificationToken
        },
        dataType: "text",
        success: function(response) {
            for(let i = 0; i < intervals.length; i++) {
                clearInterval(intervals[i].interval);
            }
            intervals = [];
            $("#auctions").html(response);
        },
        error: function(response) {
            alert(response);
        }
    }
    )
}

function purchaseTokens(amount) {
    var verificationToken = $("input[name='__RequestVerificationToken']").val();

    $.ajax({
        type: "POST",
        url: "/User/PurchaseTokens",
        data: {
            "amount": amount,
            "__RequestVerificationToken": verificationToken
        },
        dataType: "text",
        success: function(response) {
            $("#orderConfirmation").html(response);
        },
        error: function(response) {
            alert(response);
        }
    })
}

function bid(auctionId) {
    auctionID = auctionId;

    var verificationToken = $("input[name='__RequestVerificationToken']").val();

    $.ajax({
        type: "POST",
        url: "/Auction/Bid",
        data: {
            "auctionId": auctionId,
            "__RequestVerificationToken": verificationToken
        },
        success: function(response) {
            for(let i = 0; i < intervals.length; i++) {
                if(intervals[i].auctionId == auctionId) {
                    clearInterval(intervals[i].interval);
                }
            }
            connection.invoke("Bid", auctionId).catch(handleError);
        },
        error: function(response) {
            alert(response);
        }
    })
}

connection.on (
    "UpdateAuction",
    (auctionId) => {
        $.ajax({
            type: "GET",
            url: "/Auction/GetAuction?auctionId=" + auctionId,
            dataType: "text",
            success: function(response) {
                for(let i = 0; i < intervals.length; i++) {
                    if(intervals[i].auctionId == auctionId) {
                        clearInterval(intervals[i].interval);
                    }
                }
                let divId = "#auction" + auctionId;
                $(divId).html(response);
            },
            error: function(response) {
                alert(response);
            }
        })
    }
)