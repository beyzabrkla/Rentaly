<?php
$subject = 'New Customer Order'; // Subject of your email
$to = 'contact@designesia.com';  //Recipient's E-mail

$emailTo = $_POST['email'];
$name = $_POST['name'];
$email = $_POST['email'];
$phone = $_POST['phone'];
$msg = $_POST['message'];
$vehicle_type = $_POST['vehicle_type'];
$pickup_location = $_POST['pickup_location'];
$pickup_date = $_POST['pickup_date'];
$pickup_time = $_POST['pickup_time'];
$destination = $_POST['destination'];
$return_date = $_POST['return_date'];
$return_time = $_POST['return_time'];

$email_from = $name. ' ' . '<'.$email.'>';

$headers = "MIME-Version: 1.1";
$headers .= "Content-type: text/html; charset=iso-8859-1";
$headers .= "From: ".$name.'<'.$email.'>'."\r\n"; // Sender's E-mail
$headers .= "Return-Path:"."From:" . $email;

$message .= 'Name : ' . $name . "\n";
$message .= 'Email : ' . $email . "\n";
$message .= 'Phone : ' . $phone . "\n";
$message .= 'Vehicle : ' . $vehicle_type . "\n";
$message .= 'Pick Up Location : ' . $pickup_location . "\n";
$message .= 'Destination : ' . $destination . "\n";
$message .= 'Pick Up Date & Time : ' . $pickup_date . " " . $pickup_time . "\n";
$message .= 'Return Date & Time : ' . $return_date . " " . $return_time . "\n";
$message .= 'Message : ' . $msg;

if (@mail($to, $subject, $message, $email_from))
{
	// Transfer the value 'sent' to ajax function for showing success message.
	echo 'sent';
}
else
{
	// Transfer the value 'failed' to ajax function for showing error message.
	echo 'failed';
}
?>