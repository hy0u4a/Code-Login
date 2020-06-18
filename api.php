<?php
header('Content-Type: text/html; charset=UTF-8');
error_reporting(0);
define('EncryptKEY', '516546235146532465243423544253422523452');
define('KEY_128', substr(EncryptKEY, 0, 128 / 8));
define('KEY_256', substr(EncryptKEY, 0, 256 / 8));
date_default_timezone_set("Asia/Seoul");

if($_GET['mode'] == 'login'){
	if(file_exists('Code/' . $_GET['Code']))
	{
		$DATE_NOW = date_create(date("Y-m-d H:i:s"));
		$fp = fopen('Code/' . $_GET['Code'], 'r');
		$fileread = fgets($fp);
		$filesp = explode("|", $fileread);
		if($filesp[2] == '0')	//벤여부 확인
		{
			if($filesp[1] == '1')	//코드 사용 여부 확인
			{
				if($filesp[3] == $_GET['COM'])	//하드번호 확인
				{
					$datelimit = strtotime($filesp[0]) - strtotime(date("Y-m-d H:i:s"));
				    if($datelimit >= 0)
					{
						$EXPIRE_DATE = date_create(date($filesp[0]));
						$diff = date_diff($DATE_NOW, $EXPIRE_DATE);
						die($diff->days . "Days " . $diff->h . "Hours ". $diff->i . "Mins" . '/U/');
				    }else{
			    		die('Expired Code / ' . $datelimit);
			    	}
				}else{
					die('Please Unlock your Code');
				}
			}else if($filesp[1] == 0)	//사용안된 코드
			{
				if ($filesp[0] == "Unlimited") 
				{
					$timeadded = date("Y-m-d H:i:s", strtotime("+100 years"));
				}
				else if ($filesp[0] == "Test")
				{
					$timeadded = date("Y-m-d H:i:s", strtotime("+60 minutes"));
				}
				else
				{
					$timeadded = date("Y-m-d H:i:s", strtotime("+" . $filesp[0]));
				}
				$fmp = fopen('Code/' . $_GET['Code'], 'w');
				fwrite($fmp, $timeadded . "|1|0|" . $_GET['COM'] . "|2|" .	$filesp[5]);
				fclose($fmp);
				
				$EXPIRE_DATE = date_create(date($timeadded));
				$diff = date_diff($DATE_NOW, $EXPIRE_DATE);
				die($diff->days . "Days " . $diff->h . "Hours ". $diff->i . "Mins" . '/U/');
			}else{
				die('Unknown Error');
			}
		}else{
			die('BANNED Code');
		}
	}else{
		die('Unknown Serial');
	}
}

else if($_GET['mode'] == 'unlock'){//하드락해제
	if(file_exists('Code/' . $_GET['Code'])){
		$fp = fopen('Code/' . $_GET['Code'], 'r');
		$fread = explode("|", fgets($fp));
		$intda = intval($fread[4]);
		if($intda > 0){
			$fopd = fopen('Code/' . $_GET['Code'], 'w');
			fwrite($fopd, $fread[0] . '|' . $fread[1] . '|' . $fread[2] . '|' . $_GET['COM'] . '|' . strval($intda - 1) . '|' . $fread[5] . '|');
			die('Unlocked Code. Remaining Number : ' . strval($intda - 1));
			//die('하드번호를 ' . $_GET['COM'] . '으로 변경하셨습니다. 남은 횟수 : ' . strval($intda - 1));
		}else{
			die('Can\'t Unlock Code');
		}
	}else{
		die('Unknown Serial');
	}
}
?>