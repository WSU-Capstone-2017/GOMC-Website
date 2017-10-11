<?php
session_start();	
$name = $_GET['name'];
$email = $_GET['email'];
$message = $_GET['message'];
$subject = 'Quasar New Message, from ' . $name;
if(strtolower($_REQUEST['code']) == strtolower($_SESSION['random_number']))
{
$TO = "wowthemesnet@gmail.com";
$h = "From: " . $email;
$content = "$name ($email) sent you the following message :\n\n$message";
mail($TO, $subject, $content, $h);		
	echo 1;		
}	
else
{
	echo 0; // invalid code
}
?>