import {HeroSection} from "../components/home/HeroSection.tsx";
import {useAuth} from "../hooks/useAuth.ts";

function HomePage() {
    const { user } = useAuth();
    return (
        <>
            <HeroSection user={user}/>
        </>
    )
}

export default HomePage;