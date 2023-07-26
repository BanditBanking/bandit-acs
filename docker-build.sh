#!/bin/sh
convert_to_pascal_case() {
    echo "$1" | sed 's/\<\([[:lower:]]\)\([[:alnum:]]*\)/\u\1\2/g'
}

convert_to_snake_case() {
    echo "$1" | sed -e 's/\([A-Z]\)/_\L\1/g' | sed -e 's/^_//'
}

convert_to_kebab_case() {
    echo "$1" | sed -e 's/\([A-Z]\)/-\L\1/g' | sed -e 's/^-//'
}

convert_to_camel_case() {
    echo "$1" | sed -e 's/\([A-Z]\)/ \1/g' | sed -e 's/^\ //g' | awk '{for(i=1;i<=NF;i++){if(i==1){printf "%s", tolower($i)}else{printf "%s", toupper(substr($i,1,1)) substr($i,2)}}}'
}

get_project_name_from_git() {
    git remote -v | grep fetch | awk '{print $2}' | sed 's/.*\///' | sed 's/\.git//'
}

get_project_name() { 
    if [ -z "$1" ]; then
        set -- "kebab"
    fi

    if [ -f project-name.txt ]; then
        cat project-name.txt
    else
        case $1 in
            "pascal")
                convert_to_pascal_case $(get_project_name_from_git)
                ;;
            "snake")
                convert_to_snake_case $(get_project_name_from_git)
                ;;
            "kebab")
                convert_to_kebab_case $(get_project_name_from_git)
                ;;
            "camel")
                convert_to_camel_case $(get_project_name_from_git)
                ;;
            *)
                get_project_name_from_git
                ;;
        esac
    fi
}

get_project_version() {
    if [ -f version.txt ]; then 
        cat version.txt | tr -d '[:space:]'
    else
        git describe --tags --abbrev=0
    fi
}

# Get the name of the project
PROJECT_NAME=$(get_project_name)
PROJECT_VERSION=$(get_project_version)

DOCKER_IMAGE_NAME="$PROJECT_NAME:$PROJECT_VERSION"
DOCKER_ARCHIVE_NAME="$PROJECT_NAME-$PROJECT_VERSION"

CURRENT_DIRECTORY=$(pwd)
OUTPUT_TAR="$CURRENT_DIRECTORY/$DOCKER_ARCHIVE_NAME.tar"

echo "Building $PROJECT_NAME version $PROJECT_VERSION"

docker build . -t $DOCKER_IMAGE_NAME
docker save $DOCKER_IMAGE_NAME --output $OUTPUT_TAR
