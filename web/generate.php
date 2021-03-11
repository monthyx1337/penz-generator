<?php

// disable error reporting.
error_reporting(E_ERROR | E_PARSE);

// grab token.
$token= htmlspecialchars($_GET["token"]);

// no token was set.
if ($token == "")
    die("bad request");

// open token combo.
$myfile = fopen("tokens.txt", "a+") or die("error");

// add the new generated token from client.
fwrite($myfile, $token.("\n"));

// close the file.
fclose($myfile);

?>