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
    elif (( "$option" == "5" )); then
        if [ -f ../presentacion/PresentacionMoogle.pdf ] ; then
            echo "You've chosen option 5"
            cd ../presentacion
            start PresentacionMoogle.pdf
        else
            cd ../presentacion
            pdflatex PresentacionMoogle.tex
            start PresentacionMoogle.pdf
        fi
    elif (( "$option" == "6" )); then
        echo "You've chosen option 6" 
        cd ../informe
        rm InformeMoogle.aux
        rm InformeMoogle.log
        cd ../presentacion    
        rm PresentacionMoogle.aux
        rm PresentacionMoogle.log
        echo "The aux archives have been deleted succesfully"
    else
        echo "Invalid Option" 
    fi
done 