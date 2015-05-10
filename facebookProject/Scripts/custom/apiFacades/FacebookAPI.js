// This is called with the results from from FB.getLoginStatus().
document.addEventListener('DOMContentLoaded', init, false);
function init() {
    function statusChangeCallback(response) {
        // console.log('statusChangeCallback');
        //console.log(response);
        // The response object is returned with a status field that lets the
        // app know the current login status of the person.
        // Full docs on the response object can be found in the documentation
        // for FB.getLoginStatus().
        if (response.status === 'connected') {
            //getMe();
            addStatus();

            getNewsFeed();

            // Logged into your app and Facebook.
        } else if (response.status === 'not_authorized') {
            // The person is logged into Facebook, but not your app.
            document.getElementById('status').innerHTML = 'Please log ' +
              'into this app.';
        } else {
            // The person is not logged into Facebook, so we're not sure if
            // they are logged into this app or not.
            FB.login(function (response) {
                addStatus();

                getNewsFeed();
                //addStatus();
                //console.log(response.authResponse.accessToken);
            }, { scope: 'public_profile,email,user_friends,publish_actions,user_events,user_photos,user_status,user_videos,read_stream' });
        }
    }

    // This function is called when someone finishes with the Login
    // Button.  See the onlogin handler attached to it in the sample
    // code below.
    function checkLoginState() {
        FB.getLoginStatus(function (response) {
            statusChangeCallback(response);
        });
    }

    window.fbAsyncInit = function () {
        FB.init({
            appId: '457596417728680',
            cookie: true,  // enable cookies to allow the server to access 
            // the session
            xfbml: true,  // parse social plugins on this page
            version: 'v2.2' // use version 2.2
        });

        // Now that we've initialized the JavaScript SDK, we call 
        // FB.getLoginStatus().  This function gets the state of the
        // person visiting this page and can return one of three states to
        // the callback you provide.  They can be:
        //
        // 1. Logged into your app ('connected')
        // 2. Logged into Facebook, but not your app ('not_authorized')
        // 3. Not logged into Facebook and can't tell if they are logged into
        //    your app or not.
        //
        // These three cases are handled in the callback function.

        FB.getLoginStatus(function (response) {
            statusChangeCallback(response);
        });

    };

    // Load the SDK asynchronously
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

    function getMe(retn) {
        FB.api('/me', function (response) {
            //console.log(JSON.stringify(response));
            retn(response);
        });
    }

    function publishToFacebook(body) {
        FB.api('/me/feed', 'post', { message: body }, function (response) {

        });
        return false;
    }

    function publishToFacebookWithPicture(body,link) {
        FB.api('/me/feed', 'post',
            { message: body },
            { picture: link },
            function (response) {

        });
        return false;
    }

    function addStatus() {
        var start = document.getElementById("facebook");
        var div = document.createElement("div");
        var statusDiv = document.createElement("div");
        var pictureDiv = document.createElement("div");
        div.setAttribute('id', 'statusUpdate');
        var t = document.createElement("label");
        var updateStatusL = document.createTextNode("Update Status: ");
        t.appendChild(updateStatusL);

        t.setAttribute('value', "Test");
        
        t.value = "Update Status"
        t.setAttribute('class', 'updateLabel');

        var span = document.createElement("span");
        span.setAttribute('class', 'mySpan');
        var i = document.createElement("input"); //input element, text
        i.setAttribute('type', "text");
        i.setAttribute('name', "username");
        i.setAttribute('class', 'input');
        span.appendChild(i);
        var pLinkLabel = document.createElement("label");

        pLinkLabel.setAttribute('value', "pLinkLabel");
        pLinkLabel.value = "Upload Picture";
        var uploadPictureL = document.createTextNode("Upload Picture: ");
        pLinkLabel.appendChild(uploadPictureL);
        pLinkLabel.setAttribute('class', 'updateLabel');

        var pSpan = document.createElement("span");
        pSpan.setAttribute('class', 'mySpan');

        var pictureLink = document.createElement("input");
        pictureLink.setAttribute('type', 'text');
        pictureLink.setAttribute('name', 'pictureLink');
        pictureLink.setAttribute('class', 'input');
        pSpan.appendChild(pictureLink);

        var s = document.createElement("input"); //input element, Submit button
        s.setAttribute('type', "submit");
        s.setAttribute('value', "Publish");
        
        s.addEventListener('click', function () {
            var pLink = pictureLink.value;
            var message = i.value;
            if (pLink != "") {
                if (validUrl(pLink)) {
                    publishToFacebookWithPicture(message, pLink);
                    getMe(function (me) {
                        addNewsItem(me,true, function (div2) {
                            console.log("Adding Picture");
                            addPicture(pLink, div2);
                        });
                    });
                } else {
                    alert("Invalid Picture URL");
                }
            } else {
                publishToFacebook(message);
                getMe(function (me) {
                    addNewsItem(me,true, function (newsItem) {
                        addMessage(message, newsItem)
                    });
                });
            }
            i.value = "";
            pictureLink.value = "";
            FB.ui({
                method: 'send',
                link: 'http://thenosebook.tk',
            });

        }, true);

        statusDiv.appendChild(t);
        statusDiv.appendChild(span);
        pictureDiv.appendChild(pLinkLabel);
        pictureDiv.appendChild(pSpan);
        div.appendChild(statusDiv);
        div.appendChild(pictureDiv);
        div.appendChild(s);

        start.appendChild(div);
    }

    function getNewsFeed() {
        /* make the API call */
        FB.api(
            "/me/home",
            { fields: 'story,message,from,type,link' },
            {sence:'last week'},
            //{ filter: 'nf' },
            {limit:2},
            //fields:story,message,from
            //filter: other
            function (response) {
                console.log(response);
                if (response && !response.error) {
                    displayStatus(response, 0, function () { });
                }
            }
        );
    }

    function displayStatus(response, i, callback) {
        //console.log(response);
        var obj = response['data'][i];
        if (obj == undefined) {
            return;
        }
        if (obj.type == "link") {
            addNewsItem(obj.from,false, function (div) {
                addPicture(obj.link,div);
            });

        } else if (obj.type == "status") {
            addNewsItem(obj.from,false, function (newsItem) {
                addMessage(obj.message,newsItem)
            });
        }
        else if (obj.message != undefined) {
            addStatusFeed(obj.message);
        }
        if (response['data'][i+1] != undefined) {
            displayStatus(response, i+1, callback);
        } else {//if (response.paging.cursors.after) {
            
            $.getJSON(response.paging.next, function (response) {
                if (response.length != 0) {
                    displayStatus(response, 0, callback);
                }
            });
            //console.log(FB.api("/me/home/",nextPage));
            //var params = jQuery.deparam.querystring(nextPage);
            //console.log(JSON.stringify(params, null, 2));
            //FB.api('/me/home', params, displayStatus(response,0,function(){}));
            //displayStatus(response.paging.next, 0, callback);
        }
    }

    function getUser(userID, retn) {
        var call = "/" + userID + "/picture";
        FB.api(
            call,
            function (response) {
                if (response && !response.error) {
                    retn(response.data.url);
                }
            }
        );
    }

    function addStatusFeed(message) {
        var fromLabel = document.createElement("lebel");
        var fromUserLabel = document.createTextNode("lebel");
        var from = document.createTextNode("From: ");
        fromLabel.appendChild(from);
        fromLabel.appendChild(fromUserLabel);

        var messLabel = document.createElement("label");
        var mess = document.createTextNode("Message: ");
        var messageLabel = document.createTextNode(message);
        messLabel.appendChild(mess);
        messLabel.appendChild(messageLabel);

        document.getElementById("newsfeed").appendChild(fromLabel);
        document.getElementById("newsfeed").appendChild(messLabel);
    }

    function addNewsItem(user,order, callback) {
        var status = document.createElement("div");
        status.setAttribute("id", "newsItem");
        var fromLabel = document.createElement("label");

        var imgUrl = "";
        getUser(user.id, function (url) {
            var img = document.createElement("img");
            img.src = url;
            img.setAttribute("id", "fbProfilePic");

            status.appendChild(img);

            var fromUserLabel = document.createTextNode(user.name);
            fromLabel.appendChild(fromUserLabel);
            fromLabel.setAttribute("id", "name");
                  
            status.appendChild(fromLabel);
            var doc = document.getElementById("newsfeed");
            console.log(doc.childNodes);
            if (doc.childNodes[0] == undefined || !order) {
                doc.appendChild(status);
            } else {
                doc.insertBefore(status, doc.childNodes[1]);
            }
            //document.getElementById("newsfeed").appendChild(status);
            console.log("just appended child to newsfeed.")
            callback(status);
        });  

    }

    function addMessage(textmessage,message) {
        var messLabel = document.createElement("label");
        messLabel.setAttribute("id", "messageTitle");
        var messTextLabel = document.createElement("label");
        var mess = document.createTextNode("Message: ");
        var messageLabel = document.createTextNode(textmessage);
        messTextLabel.setAttribute('id', 'message');
        messLabel.appendChild(mess);
        messTextLabel.appendChild(messageLabel);
        message.appendChild(messLabel);
        message.appendChild(messTextLabel);
        //document.getElementById("newsfeed").appendChild(message);
    }

    function addPicture(picture,div) {
        var start = document.getElementById("newsfeed");

        var pictureLabel = document.createElement("pLabel");
        var pLabel = document.createTextNode("Picture: ");
        pictureLabel.appendChild(pLabel);
        

        var pic = document.createElement("img");
        pic.setAttribute("src", picture);
        pic.setAttribute("id", "fbImage");

        div.appendChild(pictureLabel);
        div.appendChild(pic);
        //start.appendChild(div);

    }

    function addComment(post, i) {
        var comment = document.createElement("div");
        var fromLabel = document.createElement("label");


        var imgUrl = "";
        getUser(user.id, function (url) {
            var img = document.createElement("img");
            img.src = url;

            comment.appendChild(img);

            var fromUserLabel = document.createTextNode(user.name);
            fromLabel.appendChild(fromUserLabel);

            comment.appendChild(fromLabel);
          //  document.getElementById("newsfeed").appendChild(comment);
            callback();
        });
    }

    function validUrl(str) {
        var pattern = new RegExp('^(https?:\\/\\/)?' + // protocol
        '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' + // domain name
        '((\\d{1,3}\\.){3}\\d{1,3}))' + // OR ip (v4) address
        '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // port and path
        '(\\?[;&a-z\\d%_.~+=-]*)?' + // query string
        '(\\#[-a-z\\d_]*)?$', 'i'); // fragment locator
        if (!pattern.test(str)) {
            return false;
        } else {
            return true;
        }
    }



}

