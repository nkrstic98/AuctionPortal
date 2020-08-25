// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//enable tooltip
$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip();
});

// Add the following code if you want the name of the file appear on select
$(".custom-file-input").on(
    "change", function() {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    }
);

//create new signalR connection
var connection =  new signalR.HubConnectionBuilder().withUrl("/update").build();

function handleError(error) {
    alert(error);
}

//start the connection
connection.start().catch(handleError);

//AJAX request for opening auctions
//Gets called every 30s
setInterval(function() {
    $.ajax({
        url: "/Auction/openAuctions"
    })
}, 30 * 1000);

//AJAX request for filtering auctions
function filter(wonAuctions) {
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
            "currPage": 1,
            "wonAuctions": wonAuctions,
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
    
    $.ajax({
        type: "POST",
        url: "/Auction/Search",
        data: {
            "name": auctionName,
            "minPrice": minPrice,
            "maxPrice": maxPrice,
            "state": auctionState,
            "currPage": 1,
            "second": true,
            "wonAuctions": wonAuctions,
            "__RequestVerificationToken": verificationToken
        },
        dataType: "text",
        success: function(response) {
            $("#pagination").html(response);
        },
        error: function(response) {
            alert(response);
        }
    });

    curPage = 1;
    newPage = 1;

    $("#tokenOrders").html("");
}

//AJAX request for closing auctions
//Gets called when aution's time runs out
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

//AJAX call for reseting all filters
function noFilters() {
    var verificationToken = $("input[name='__RequestVerificationToken']").val();

    $("#auctionName").val("");
    $("#minPrice").val("");
    $("#maxPrice").val("");
    $("#auctionState").val("");

    $.ajax({
        type: "POST",
        url: "/Auction/Search",
        data: {
            "name": "",
            "minPrice": "",
            "maxPrice": "",
            "state": "",
            "currPage": 1,
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

    $.ajax({
        type: "POST",
        url: "/Auction/Search",
        data: {
            "name": "",
            "minPrice": "",
            "maxPrice": "",
            "state": "",
            "currPage": 1,
            "second": true,
            "__RequestVerificationToken": verificationToken
        },
        dataType: "text",
        success: function(response) {
            $("#pagination").html(response);
        },
        error: function(response) {
            alert(response);
        }
    });

    curPage = 1;
    newPage = 1;
}

//AJAX request for PayPal token purchase
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

//AJAX request that gets called when user bids on auction
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

var curPage = 1, newPage = 1;

//add or remove active and disabled classes for pagination list
function changePageFront(pageNum, numPages) {
    if(pageNum == 0) {
        curPage = newPage;
        newPage--;
    }
    else {
        if(pageNum == numPages + 1) {
            curPage = newPage;
            newPage++;
        }
        else {
            curPage = newPage;
            newPage = pageNum;
        }
    }

    newCur = "#page" + newPage;
    oldCur = "#page" + curPage;

    $(oldCur).toggleClass("active");
    $(newCur).toggleClass("active");

    if(newPage == 1) {
        $("#pagePrev").toggleClass("disabled");
    }

    if(newPage != 1 && curPage == 1) {
        $("#pagePrev").toggleClass("disabled")
    }

    if(newPage == numPages) {
        $("#pageNext").toggleClass("disabled");
    }

    if(curPage == numPages && newPage < numPages) {
        $("#pageNext").toggleClass("disabled");
    }
}

//AJAX request for showing different page with auctions
function changePage(pageNum, numPages) {
    var verificationToken = $("input[name='__RequestVerificationToken']").val();

    window.scrollTo({ top: 0, behavior: 'smooth' });

    changePageFront(pageNum, numPages);

    var auctionName = $("#auctionName").val();
    var minPrice = $("#minPrice").val();
    var maxPrice = $("#maxPrice").val();
    var auctionState = $("#auctionState").val();

    $.ajax({
        type: "POST",
        url: "/Auction/Search",
        data: {
            "name": auctionName,
            "minPrice": minPrice,
            "maxPrice": maxPrice,
            "state": auctionState,
            "currPage": newPage,
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
    });
}

//AJAX request that gets all orders made by logged user
function getTokenOrders(page) {
    $.ajax({
        type: "GET",
        url: "/User/GetTokenOrders/pageNum=" + page,
        dataType: "text",
        success: function(response) {
            $("#tokenOrders").html(response);
        },
        error: function(response) {
            alert(response);
        }
    })

    $("#auctions").html("");
    $("#pagination").html("");
}

//Log out user when closing browser if User == Admin
window.onbeforeunload = function(evt) {
    var verificationToken = $("input[name='__RequestVerificationToken']").val();

    if(adminLogged) {
        $.ajax({
            type: "POST",
            url: "/User/Logout",
            data: {
                "__RequestVerificationToken": verificationToken
            },
            success: function (response) {
                alert(response);
            },
            error: function(response) {
                alert(response);
            }
        })
    }
}

//invoke AJAX request for changing Auction View in real-time for all users
//Gets called when someone bids on auction, or auction gets closed
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

                $.ajax({
                    type: "GET",
                    url: "/Auction/GetDetails?auctionId=" + auctionId,
                    dataType: "text",
                    success: function(response) {
                        divId = "#auctionDetails" + auctionId;
                        $(divId).html(response);
                    },
                    error: function(response) {
                        alert(response);
                    }
                })
            },
            error: function(response) {
                alert(response);
            }
        })
    }
)