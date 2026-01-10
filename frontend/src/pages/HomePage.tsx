import {HeroSection} from "../components/home/HeroSection.tsx";
import type {UserDto} from "../Models/users.type.ts";
interface Props {
    user: UserDto | null;
}

function HomePage({ user }: Props) {
    return (
        <>
            <HeroSection user={user}/>
        </>
    )
}

export default HomePage;