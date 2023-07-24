echo "1. run"
echo "2. report"
echo "3. slide"
echo "4. show_report"
echo "5. show_slide"
echo "6. clean"
echo "If you want to exit, press Ctrl + C"
while true
do

    read option
    
    if (( "$option" == "1" )); then 
        echo "You've chosen option 1"
        dotnet watch run --project ../moogle-main/MoogleServer
    elif (( "$option" == "2" )); then
        echo "You've chosen option 2"
        cd ../informe
        pdflatex InformeMoogle.tex
        cd ../script
    elif (( "$option" == "3" )); then
        echo "You've chosen option 3"
        cd ../presentacion
        pdflatex PresentacionMoogle.tex
    elif (( "$option" == "4" )); then
        if [ -f ../informe/InformeMoogle.pdf ]; then    
            echo "You've chosen option 4"
            cd ../informe
            start InformeMoogle.pdf
        else
            cd ../informe
            pdflatex InformeMoogle.tex
            start InformeMoogle.pdf
        fi
        cd ../script
    elif (( "$option" == "5" )); then
        if [ -f ../presentacion/PresentacionMoogle.pdf ]; then
            echo "You've chosen option 5"
            cd ../presentacion
            start PresentacionMoogle.pdf
        else
            cd ../presentacion
            pdflatex PresentacionMoogle.tex
            start PresentacionMoogle.pdf
        fi
        cd ../script
    elif (( "$option" == "6" )); then
        echo "You've chosen option 6" 
        cd ../informe
        if [ -f InformeMoogle.aux ]; then 
            rm InformeMoogle.aux
        fi
        if [ -f InformeMoogle.log ]; then
            rm InformeMoogle.log
        fi
        cd ../presentacion 
        if [ -f PresentacionMoogle.aux ]; then   
            rm PresentacionMoogle.aux
        fi
        if [ -f PresentacionMoogle.log ]; then 
            rm PresentacionMoogle.log
        fi
        cd ../moogle-main/MoogleServer
        if [ -d bin ]; then
            rm -R bin
        fi
        if [ -d obj ]; then
            rm -R obj
        fi
        cd ../MoogleEngine
        if [ -d bin ]; then
            rm -R bin
        fi
        if [ -d obj ]; then
            rm -R obj
        fi
        echo "The aux archives have been deleted succesfully"
        cd ../../script
    else
        echo "Invalid Option" 
    fi
done 