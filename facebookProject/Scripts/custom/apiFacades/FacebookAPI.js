// This is called with the results from from FB.getLoginStatus().
document.addEventListener('DOMContentLoaded', init, false);
function init() {
function statusChangeCallback(response) {
      console.log('statusChangeCallback');
      console.log(response);
      // The response object is returned with a status field that lets the
      // app know the current login status of the person.
      // Full docs on the response object can be found in the documentation
      // for FB.getLoginStatus().
      if (response.status === 'connected') {
          addStatus();
          getMe();
          //publishToFacebook("test");
          console.log(response.authResponse.accessToken);
          // Logged into your app and Facebook.
          testAPI();
      } else if (response.status === 'not_authorized') {
          // The person is logged into Facebook, but not your app.
          document.getElementById('status').innerHTML = 'Please log ' +
            'into this app.';
      } else {
          // The person is not logged into Facebook, so we're not sure if
          // they are logged into this app or not.
          FB.login(function (response) {
              //addStatus();
              console.log(response.authResponse.accessToken);
          }, {scope:'public_profile,email,user_friends,publish_actions,user_events,user_photos,user_status,user_videos,read_stream'});
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

        // Here we run a very simple test of the Graph API after login is
        // successful.  See statusChangeCallback() for when this call is made.
        function testAPI() {
            console.log('Welcome!  Fetching your information.... ');
            FB.api('/me', function (response) {
                console.log('Successful login for: ' + response.name);
                document.getElementById('status').innerHTML =
                  'Thanks for logging in, ' + response.name + '!';
            });
        }

        function getMe() {
            FB.api('/me', function (response) {
                console.log(JSON.stringify(response));
            })
        }
        
            function publishToFacebook(body) {
                //ev.preventDefault();
                console.log("In publish :)");
                FB.api('/me/feed', 'post', { message: body }, function (response) {
                    if (!response || response.error) {
                        console.log("Post Error :(");
                        //alert('Error occured');
                    } else {
                        console.log("Posted :) :)");
                        //a//lert('Post ID: ' + response.id);
                    }
                });
                return false;
            }

            function addStatus() {
                
                var t = document.createElement("label");

                t.setAttribute('value', "Test");
                t.value = "Update Status"

                //var f = document.createElement("form");



                var i = document.createElement("input"); //input element, text
                i.setAttribute('type', "text");
                i.setAttribute('name', "username");
                var s = document.createElement("input"); //input element, Submit button
                s.setAttribute('type', "submit");
                s.setAttribute('value', "Publish");
                s.addEventListener('click', function () {
                    publishToFacebook(i.value);
                }, true);

                document.getElementById("facebook").appendChild(t);
                document.getElementById("facebook").appendChild(i);
                document.getElementById("facebook").appendChild(s);


            }
        }
        