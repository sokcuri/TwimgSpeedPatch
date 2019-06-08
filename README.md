# 트위터 이미지 속도 패치
Hosts 파일을 수정해 트위터 이미지 로딩 속도를 비약적으로 향상시키는 트위터 속도 패치입니다.

* [패치 받으러 가기](https://github.com/sokcuri/TwimgSpeedPatch/releases)
* [사용중 문제점 보고하기](https://github.com/sokcuri/TwimgSpeedPatch/issues)
* [트위터로도 피드백을 받고 있습니다](https://twitter.com/sokcuri)

## 패치 방법
패치를 다운받고 패치하기를 누르면 패치되고, 제거하기를 누르면 패치가 제거됩니다. 현재 여러개의 이미지 서버 ip가 리스팅되어 있고, 이 중 하나를 골라 패치하게 됩니다.

윈도우 외 OS 사용자는 [server_ip.json](https://github.com/sokcuri/TwimgSpeedPatch/blob/master/data/server_ip.json)을 참고하셔서 직접 Hosts 설정을 고치실 수 있습니다. 호스트는 `pbs.twimg.com`을 사용합니다.

## 참고사항
* Hosts 파일을 고치기 때문에 백신에서 오진을 하는 경우가 있습니다. 이 경우 오진 신고와 함께 [이슈](https://github.com/sokcuri/TwimgSpeedPatch/issues)를 남겨주시면 큰 도움이 됩니다.
* 프로그램을 실행할때 트위터 이미지 서버 주소 리스트를 받아오므로 인터넷 연결이 필요합니다.
* 프로그램 사용 중 따로 개인정보를 수집하거나 기능 이외의 일을 하지 않으니 안심하셔도 됩니다.
* 패치는 한번 적용시 영구히 적용되나, 백신이나 인터넷뱅킹 프로그램에 의해 롤백되는 경우가 있습니다. 이 경우 다시 패치를 해 주시면 적용됩니다.
